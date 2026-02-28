using ValetaxTest.Application.Responses;

public interface ITreeService
{
    Task<NodeResponse> GetTreeAsync(string treeName, CancellationToken cancellationToken = default);

    Task<NodeResponse> CreateNodeAsync(
        string treeName,
        string nodeName,
        long? parentNodeId,
        CancellationToken cancellationToken = default);

    Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken = default);

    Task RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken = default);
}