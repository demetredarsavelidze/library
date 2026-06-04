using LibraryManagement.Application.Abstractions.Authentication;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserManagerAdapter _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IUserManagerAdapter userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
    {
        var role = string.IsNullOrWhiteSpace(dto.Role) ? Roles.User : dto.Role.Trim().ToLowerInvariant();
        await ValidateRegistrationAsync(dto.UserName, dto.Password, role, cancellationToken);

        var user = await _userManager.CreateAsync(dto.UserName.Trim(), dto.Password, role, cancellationToken);
        return CreateResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
    {
        var validation = new ValidationCollector();

        if (string.IsNullOrWhiteSpace(dto.UserName))
        {
            validation.Add(nameof(dto.UserName), "UserName is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            validation.Add(nameof(dto.Password), "Password is required.");
        }

        validation.ThrowIfAny();

        var user = await _userManager.ValidateCredentialsAsync(dto.UserName.Trim(), dto.Password, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        return CreateResponse(user);
    }

    private async Task ValidateRegistrationAsync(
        string userName,
        string password,
        string role,
        CancellationToken cancellationToken)
    {
        var validation = new ValidationCollector();

        if (string.IsNullOrWhiteSpace(userName))
        {
            validation.Add(nameof(RegisterRequestDto.UserName), "UserName is required.");
        }
        else if (await _userManager.UserNameExistsAsync(userName.Trim(), cancellationToken))
        {
            validation.Add(nameof(RegisterRequestDto.UserName), "UserName is already taken.");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            validation.Add(nameof(RegisterRequestDto.Password), "Password must contain at least 8 characters.");
        }

        if (!Roles.Supported.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            validation.Add(nameof(RegisterRequestDto.Role), "Role must be admin or user.");
        }

        validation.ThrowIfAny();
    }

    private AuthResponseDto CreateResponse(ApplicationUser user)
    {
        var token = _jwtTokenService.GenerateToken(user);
        return new AuthResponseDto
        {
            UserName = user.UserName,
            Role = user.Role,
            Token = token.Token,
            ExpiresAtUtc = token.ExpiresAtUtc
        };
    }
}
