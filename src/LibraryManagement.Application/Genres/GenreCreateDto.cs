using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Genres;

public sealed class GenreCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
