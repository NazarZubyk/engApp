using Back.Features.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("admin/login")]
    public async Task<ActionResult<TokenResponse>> AdminLogin(
        [FromBody] AdminLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.AdminLoginAsync(request, cancellationToken);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [HttpPost("user/login")]
    public async Task<ActionResult<TokenResponse>> UserLogin(
        [FromBody] UserLoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.UserLoginAsync(request, cancellationToken);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}
