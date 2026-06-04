using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Abstractions.Authentication;

public interface IUserManagerAdapter
{
    Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);

    Task<ApplicationUser> CreateAsync(string userName, string password, string role, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> ValidateCredentialsAsync(string userName, string password, CancellationToken cancellationToken = default);
}
