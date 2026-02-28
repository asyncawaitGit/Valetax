using ValetaxTest.Domain.Entities;

namespace ValetaxTest.Application.Interfaces;

public interface INodeRepository
{
    Task<Node?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Node>> GetTreeNodesAsync(Guid treeId, CancellationToken cancellationToken = default);

    Task<bool> ExistsInTreeAsync(Guid treeId, CancellationToken cancellationToken = default);

    void Add(Node node);

    void Update(Node node);

    void Remove(Node node);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}