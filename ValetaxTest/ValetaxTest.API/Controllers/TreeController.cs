using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ValetaxTest.API.Controllers;

[ApiController]
[Authorize]
[Route("api.user.tree")]
public class TreeController : ControllerBase
{
    private readonly ITreeService _treeService;

    public TreeController(ITreeService treeService)
    {
        _treeService = treeService;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetTree([FromQuery] string treeName)
    {
        var tree = await _treeService.GetTreeAsync(treeName);
        return Ok(tree);
    }
}
