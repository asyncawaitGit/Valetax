namespace ValetaxTest.Application.Requests;

public class CreateNodeRequest
{
    public string TreeName { get; set; } = string.Empty;
    public long? ParentNodeId { get; set; }
    public string NodeName { get; set; } = string.Empty;
}