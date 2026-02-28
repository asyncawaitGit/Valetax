namespace ValetaxTest.Domain.ValueObjects;

public sealed class NodeName
{
    public string Value { get; }

    public NodeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be empty");

        Value = value;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => Equals(obj as NodeName);

    public bool Equals(NodeName? other) => other != null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
