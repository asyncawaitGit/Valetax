namespace ValetaxTest.Application.Responses;

public class NodeResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TreeId { get; set; }
    public Guid? ParentId { get; set; }
    public List<NodeResponse> Children { get; set; } = new();
}