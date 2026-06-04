namespace LibraryManagement.Domain.Entities;

public sealed class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

    public int Pages { get; set; }

    public int PublicationYear { get; set; }
}
