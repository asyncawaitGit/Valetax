namespace ValetaxTest.Application.Responses;

public class NodeResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<NodeResponse> Children { get; set; } = new();
}