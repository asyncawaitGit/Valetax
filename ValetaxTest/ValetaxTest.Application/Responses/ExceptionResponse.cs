using ValetaxTest.Application.Data;

namespace ValetaxTest.Application.Responses;

public class ExceptionResponse
{
    public string Type { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public ExceptionData Data { get; set; } = new();
}