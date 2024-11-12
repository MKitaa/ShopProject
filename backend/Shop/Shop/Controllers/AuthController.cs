using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Auth(SignInManager<IdentityUser> signInManager) : Controller
{
    [HttpGet]
    [Route("check-auth")]
    public IActionResult CheckAuth()
    {
        if (User.Identity!.IsAuthenticated)
            return Ok(new { isAuthenticated = true });
        return Unauthorized();
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IResult> Logout([FromBody] object empty)
    {
        if (empty == null) return Results.Unauthorized();
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    [HttpGet("check-role")]
    public IActionResult CheckRole()
    {
        var user = User;
        return Ok(user.IsInRole("Admin"));
    }
}
