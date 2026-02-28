namespace ValetaxTest.Application.Requests;

public class CreateNodeRequest
{
    public string NodeName { get; set; } = string.Empty;
    public Guid TreeId { get; set; }
    public Guid? ParentId { get; set; }
}