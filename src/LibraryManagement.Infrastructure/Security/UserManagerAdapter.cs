using LibraryManagement.Application.Abstractions.Authentication;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;

namespace LibraryManagement.Infrastructure.Security;

public sealed class UserManagerAdapter : IUserManagerAdapter
{
    private readonly IUserRepository _userRepository;

    public UserManagerAdapter(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
    {
        return _userRepository.UserNameExistsAsync(userName, cancellationToken);
    }

    public Task<ApplicationUser> CreateAsync(
        string userName,
        string password,
        string role,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            PasswordHash = PasswordHasher.Hash(password),
            Role = role
        };

        return _userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<ApplicationUser?> ValidateCredentialsAsync(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUserNameAsync(userName, cancellationToken);
        if (user is null)
        {
            return null;
        }

        return PasswordHasher.Verify(password, user.PasswordHash) ? user : null;
    }
}
