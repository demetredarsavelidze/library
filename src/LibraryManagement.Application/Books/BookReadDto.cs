namespace LibraryManagement.Application.Books;

public sealed class BookReadDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public string GenreName { get; set; } = string.Empty;

    public int Pages { get; set; }

    public int PublicationYear { get; set; }

    public int BookAge { get; set; }

    public bool IsThick { get; set; }
}
