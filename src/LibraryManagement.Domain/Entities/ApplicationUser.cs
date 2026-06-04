namespace LibraryManagement.Domain.Entities;

public sealed class ApplicationUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = Roles.User;
}
