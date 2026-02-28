using ValetaxTest.Application.Interfaces;
using ValetaxTest.Application.Requests;
using ValetaxTest.Application.Responses;
using ValetaxTest.Domain.Entities;
using ValetaxTest.Domain.Exceptions;
using ValetaxTest.Domain.ValueObjects;

namespace ValetaxTest.Application.Services;

public class TreeService : ITreeService
{
    private readonly INodeRepository _nodeRepository;

    public TreeService(INodeRepository nodeRepository)
    {
        _nodeRepository = nodeRepository;
    }

    public async Task<NodeResponse> CreateNodeAsync(
        string treeName,
        string nodeName,
        long? parentNodeId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(treeName))
            throw new SecureException("Tree name cannot be empty");

        if (string.IsNullOrWhiteSpace(nodeName))
            throw new SecureException("Node name cannot be empty");

        var nodeNameValue = new NodeName(nodeName);

        if (parentNodeId.HasValue)
        {
            var parent = await _nodeRepository.GetByIdAsync(parentNodeId.Value, cancellationToken);
            if (parent == null)
                throw new SecureException($"Parent node with ID {parentNodeId} not found");

            if (parent.TreeName != treeName)
                throw new SecureException("Parent node belongs to a different tree");
        }

        var treeNodes = await _nodeRepository.GetTreeNodesAsync(treeName, cancellationToken);
        var siblings = parentNodeId.HasValue
            ? treeNodes.Where(x => x.ParentId == parentNodeId.Value)
            : treeNodes.Where(x => !x.ParentId.HasValue);

        if (siblings.Any(x => x.Name.Value == nodeName))
            throw new SecureException("Node name must be unique across all siblings");

        var node = new Node(treeName, nodeNameValue, parentNodeId);

        _nodeRepository.Add(node);
        await _nodeRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(node);
    }

    public async Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken = default)
    {
        var node = await _nodeRepository.GetByIdAsync(nodeId, cancellationToken);
        if (node == null)
            throw new SecureException($"Node with ID {nodeId} not found");

        await DeleteNodeAndDescendantsAsync(node, cancellationToken);
        await _nodeRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RenameNodeAsync(long nodeId, string newNodeName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newNodeName))
            throw new SecureException("Node name cannot be empty");

        var node = await _nodeRepository.GetByIdAsync(nodeId, cancellationToken);
        if (node == null)
            throw new SecureException($"Node with ID {nodeId} not found");

        var treeNodes = await _nodeRepository.GetTreeNodesAsync(node.TreeName, cancellationToken);
        var siblings = node.ParentId.HasValue
            ? treeNodes.Where(x => x.ParentId == node.ParentId.Value && x.Id != nodeId)
            : treeNodes.Where(x => !x.ParentId.HasValue && x.Id != nodeId);

        if (siblings.Any(x => x.Name.Value == newNodeName))
            throw new SecureException("Node name must be unique across all siblings");

        var newNodeNameValue = new NodeName(newNodeName);
        node.Rename(newNodeNameValue);

        _nodeRepository.Update(node);
        await _nodeRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<NodeResponse> GetTreeAsync(string treeName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(treeName))
            throw new SecureException("Tree name cannot be empty");

        var exists = await _nodeRepository.ExistsInTreeAsync(treeName, cancellationToken);

        if (!exists)
        {
            return await CreateNodeAsync(treeName, "Root", null, cancellationToken);
        }

        var nodes = await _nodeRepository.GetTreeNodesAsync(treeName, cancellationToken);

        var rootNodes = nodes.Where(x => !x.ParentId.HasValue).ToList();

        if (!rootNodes.Any())
            throw new SecureException($"Tree '{treeName}' has no root nodes");

        if (rootNodes.Count > 1)
        {
            // Логируем, но возвращаем первый (или можно объединить)
            // По заданию не указано, но в Swagger возвращается один MNode
        }

        return BuildTreeResponse(rootNodes.First(), nodes.ToList());
    }

    private async Task DeleteNodeAndDescendantsAsync(Node node, CancellationToken cancellationToken = default)
    {
        // Сначала удаляем всех детей (рекурсивно)
        foreach (var child in node.Children.ToList())
        {
            await DeleteNodeAndDescendantsAsync(child, cancellationToken);
        }

        // Потом удаляем сам узел
        _nodeRepository.Remove(node);
    }

    private NodeResponse MapToResponse(Node node)
    {
        return new NodeResponse
        {
            Id = node.Id,
            Name = node.Name.Value,
            Children = node.Children.Select(MapToResponse).ToList()
        };
    }

    private NodeResponse BuildTreeResponse(Node root, List<Node> allNodes)
    {
        var response = MapToResponse(root);
        response.Children = allNodes
            .Where(x => x.ParentId == root.Id)
            .Select(x => BuildTreeResponse(x, allNodes))
            .ToList();
        return response;
    }
}