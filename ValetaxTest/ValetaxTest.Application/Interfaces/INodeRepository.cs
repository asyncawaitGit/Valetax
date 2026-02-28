using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Application.Interfaces;

public interface INodeRepository
{
    Task<Node?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Node>> GetTreeNodesAsync(string treeName, CancellationToken cancellationToken = default);

    Task<bool> ExistsInTreeAsync(string treeName, CancellationToken cancellationToken = default);

    void Add(Node node);

    void Update(Node node);

    void Remove(Node node);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}