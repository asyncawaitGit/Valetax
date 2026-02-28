using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Application.Interfaces;

public interface IExceptionJournalRepository
{
    Task<ExceptionJournal> AddAsync(ExceptionJournal exceptionJournal, CancellationToken cancellationToken = default);

    Task<ExceptionJournal?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExceptionJournal>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}