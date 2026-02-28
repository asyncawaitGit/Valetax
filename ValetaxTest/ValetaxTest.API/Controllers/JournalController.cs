using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValetaxTest.Application.Interfaces;

namespace ValetaxTest.API.Controllers;

[ApiController]
[Authorize]
[Route("api.user.journal")]
public class JournalController : ControllerBase
{
    private readonly IExceptionJournalRepository _journalRepository;

    public JournalController(IExceptionJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    [HttpPost("getRange")]
    public async Task<IActionResult> GetRange(
    [FromQuery] int skip,
    [FromQuery] int take,
    [FromBody] object? filter = null)
    {
        var journals = await _journalRepository.GetByDateRangeAsync(
            DateTime.MinValue,
            DateTime.MaxValue,
            CancellationToken.None);

        var paginated = journals
            .Skip(skip)
            .Take(take)
            .Select(j => new
            {
                id = j.Id,
                eventId = j.Id,
                createdAt = j.Timestamp
            })
            .ToList();

        return Ok(new
        {
            skip,
            take,
            items = paginated
        });
    }

    [HttpPost("getSingle")]
    public async Task<IActionResult> GetSingle([FromQuery] long id)
    {
        var entry = await _journalRepository.GetByIdAsync(id);
        if (entry == null)
            return NotFound();

        return Ok(new
        {
            id = entry.Id,
            eventId = entry.Id,
            createdAt = entry.Timestamp,
            text = $"Parameters: {entry.Parameters}\nStackTrace: {entry.StackTrace}"
        });
    }
}
