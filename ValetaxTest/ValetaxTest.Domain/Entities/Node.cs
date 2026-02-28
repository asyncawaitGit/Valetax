using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Domain.ValueObjects;

namespace ValetaxTest.Domain.Entities;

public class Node
{
    public Guid Id { get; private set; }

    public NodeName Name { get; private set; } = null!;

    public Guid TreeId { get; private set; }

    public Guid? ParentId { get; private set; }

    private readonly List<Node> _children = new();
    public IReadOnlyCollection<Node> Children => _children;

    private Node() { } // для ORM

    public Node(Guid treeId, NodeName name, Guid? parentId = null)
    {
        Id = Guid.NewGuid();
        TreeId = treeId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ParentId = parentId;
    }

    public void AddChild(Node child)
    {
        if (child.TreeId != TreeId)
            throw new SecureException("Child node must belong to the same tree");

        _children.Add(child);
    }

    public void Delete()
    {
        if (_children.Any())
            throw new SecureException("You have to delete all children nodes first");
    }
}
