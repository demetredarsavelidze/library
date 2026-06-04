using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Abstractions.Authentication;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(ApplicationUser user);
}
