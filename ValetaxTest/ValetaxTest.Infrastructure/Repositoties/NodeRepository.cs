using Microsoft.EntityFrameworkCore;
using ValetaxTest.Application.Interfaces;
using ValetaxTest.Domain.Entities;
using ValetaxTest.Infrastructure.Data;

namespace ValetaxTest.Infrastructure.Repositories;

public class NodeRepository : INodeRepository
{
    private readonly ValetaxTestDbContext _context;

    public NodeRepository(ValetaxTestDbContext context)
    {
        _context = context;
    }

    public async Task<Node?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Nodes
            .Include(x => x.Children)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Node>> GetTreeNodesAsync(string treeName, CancellationToken cancellationToken = default)
    {
        return await _context.Nodes
                    .Where(x => x.TreeName == treeName)
                    .Include(x => x.Children)
                    .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsInTreeAsync(string treeName, CancellationToken cancellationToken = default)
    {
        return await _context.Nodes.AnyAsync(x => x.TreeName == treeName, cancellationToken);
    }

    public void Add(Node node)
    {
        _context.Nodes.Add(node);
    }

    public void Update(Node node)
    {
        _context.Entry(node).State = EntityState.Modified;
    }

    public void Remove(Node node)
    {
        _context.Nodes.Remove(node);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}