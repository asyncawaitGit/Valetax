using System.Text;
using ValetaxTest.Application.Interfaces;
using ValetaxTest.Application.Responses;
using ValetaxTest.Domain.Exceptions;

namespace ValetaxTest.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IServiceScopeFactory scopeFactory) 
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, scopeFactory);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        IServiceScopeFactory scopeFactory)
    {
        var parameters = await GetRequestParameters(context);

        using var scope = scopeFactory.CreateScope();
        var journalRepository = scope.ServiceProvider.GetRequiredService<IExceptionJournalRepository>();

        var journalEntry = new Domain.Entities.ExceptionJournal(
            DateTime.UtcNow,
            parameters,
            exception.StackTrace ?? "No stack trace"
        );

        await journalRepository.AddAsync(journalEntry);
        await journalRepository.SaveChangesAsync();

        var response = new ExceptionResponse();

        if (exception is SecureException secureEx)
        {
            response.Type = "Secure";
            response.Id = journalEntry.Id.ToString();
            response.Data.Message = secureEx.Message;
        }
        else
        {
            response.Type = "Exception";
            response.Id = journalEntry.Id.ToString();
            response.Data.Message = $"Internal server error ID = {journalEntry.Id}";

            _logger.LogError(exception, "Unhandled exception caught. Event ID: {EventId}", journalEntry.Id);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(response);
    }

    private async Task<string> GetRequestParameters(HttpContext context)
    {
        var sb = new StringBuilder();

        if (context.Request.Query.Any())
        {
            sb.AppendLine("Query:");
            foreach (var (key, value) in context.Request.Query)
            {
                sb.AppendLine($"  {key}: {value}");
            }
        }

        if (context.Request.Method == HttpMethods.Post && context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            sb.AppendLine("Body:");
            sb.AppendLine($"  {body}");
        }

        return sb.ToString();
    }
}