using LibraryManagement.Application.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(dto, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(dto, cancellationToken);
        return Ok(response);
    }
}
