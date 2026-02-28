using Microsoft.EntityFrameworkCore;
using ValetaxTest.Application.Interfaces;
using ValetaxTest.Domain.Entities;
using ValetaxTest.Infrastructure.Data;

namespace ValetaxTest.Infrastructure.Repositories;

public class ExceptionJournalRepository : IExceptionJournalRepository
{
    private readonly ValetaxTestDbContext _context;

    public ExceptionJournalRepository(ValetaxTestDbContext context)
    {
        _context = context;
    }

    public async Task<ExceptionJournal> AddAsync(ExceptionJournal exceptionJournal, CancellationToken cancellationToken = default)
    {
        var entry = await _context.ExceptionJournals.AddAsync(exceptionJournal, cancellationToken);
        return entry.Entity;
    }

    public async Task<ExceptionJournal?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ExceptionJournals
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExceptionJournal>> GetByDateRangeAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        return await _context.ExceptionJournals
            .Where(x => x.Timestamp >= from && x.Timestamp <= to)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}