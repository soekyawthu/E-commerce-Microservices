using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var token = await HttpContext.GetUserAccessTokenAsync();
        //var items = (await HttpContext.AuthenticateAsync()).Properties?.Items;
        return Ok(new { token });
    }
}