using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Authors;

public sealed class AuthorCreateDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public DateOnly BirthDate { get; set; }
}
