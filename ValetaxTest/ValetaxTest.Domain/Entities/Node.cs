using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Domain.ValueObjects;

namespace ValetaxTest.Domain.Entities;

public class Node
{
    public long Id { get; private set; }

    public NodeName Name { get; private set; } = null!;

    public string TreeName { get; private set; } = null!;

    public long? ParentId { get; private set; }

    private readonly List<Node> _children = new();
    public IReadOnlyCollection<Node> Children => _children;

    private Node() { } // для ORM

    public Node(string treeName, NodeName name, long? parentId = null)
    {
        TreeName = treeName;
        Name = name;
        ParentId = parentId;
    }

    public void AddChild(Node child)
    {
        if (child.TreeName != TreeName)
            throw new SecureException("Child node must belong to the same tree");

        _children.Add(child);
    }

    public void Delete()
    {
        if (_children.Any())
            throw new SecureException("You have to delete all children nodes first");
    }

    public void Rename(NodeName newName)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }
}
