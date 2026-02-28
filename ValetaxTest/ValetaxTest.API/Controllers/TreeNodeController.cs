using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ValetaxTest.API.Controllers;

[ApiController]
[Authorize]
[Route("api.user.tree.node")]
public class TreeNodeController : ControllerBase
{
    private readonly ITreeService _treeService;

    public TreeNodeController(ITreeService treeService)
    {
        _treeService = treeService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] string nodeName,
        [FromQuery] long? parentNodeId)
    {
        var node = await _treeService.CreateNodeAsync(treeName, nodeName, parentNodeId);
        return Ok(node);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteNode([FromQuery] long nodeId)
    {
        await _treeService.DeleteNodeAsync(nodeId);
        return Ok();
    }

    [HttpPost("rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName)
    {
        await _treeService.RenameNodeAsync(nodeId, newNodeName);
        return Ok();
    }
}
