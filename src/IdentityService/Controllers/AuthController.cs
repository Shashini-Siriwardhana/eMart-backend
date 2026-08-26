using IdentityService.DTOs;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserDto request)
    {
        var user = await authService.RegisterAsync(request);
        if (user is null)
        {
            return BadRequest(new { message = "Username already exists." });
        }
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(UserDto request)
    {
        var token = await authService.LoginAsync(request);
        if (token is null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }
        return Ok(token);
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedEndpoint()
    {
        return Ok(new { message = "You are authenticated!" });
    }
    
    [Authorize(Roles="Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok(new { message = "You are an Admin!" });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        var result = await authService.RefreshTokensAsync(request);

        if (result is null)
        {
            return Unauthorized(new { message = "Invalid refresh token or user not found." });
        }
        return Ok(result);
    }

    
}