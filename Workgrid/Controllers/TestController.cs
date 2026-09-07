using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("protected")]
    [Authorize]
    public IActionResult Protected()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Ok(new
        {
            message = "You are authenticated.",
            userId = userId
        });
    }
}