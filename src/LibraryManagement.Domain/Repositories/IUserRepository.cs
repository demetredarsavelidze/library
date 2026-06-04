using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<ApplicationUser> AddAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
}
