using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Books;

public sealed class BookUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public int GenreId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Pages { get; set; }

    [Required]
    public int PublicationYear { get; set; }
}
