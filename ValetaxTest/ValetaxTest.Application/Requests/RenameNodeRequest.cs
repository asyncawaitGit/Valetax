namespace ValetaxTest.Application.Requests;

public class RenameNodeRequest
{
    public long NodeId { get; set; }
    public string NewNodeName { get; set; } = string.Empty;
}