namespace LibraryManagement.Application.Auth;

public sealed class AuthResponseDto
{
    public string UserName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}
