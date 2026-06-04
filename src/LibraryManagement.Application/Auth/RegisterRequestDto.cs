using System.ComponentModel.DataAnnotations;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Auth;

public sealed class RegisterRequestDto
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = Roles.User;
}
