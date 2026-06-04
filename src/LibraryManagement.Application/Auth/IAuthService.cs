namespace LibraryManagement.Application.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
}
