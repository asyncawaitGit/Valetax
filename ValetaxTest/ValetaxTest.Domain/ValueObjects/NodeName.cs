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
}
