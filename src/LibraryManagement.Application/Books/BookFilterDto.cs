namespace LibraryManagement.Application.Books;

public sealed class BookFilterDto
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public int? MinPages { get; set; }

    public int? MaxPages { get; set; }

    public int? GenreId { get; set; }

    public int? AuthorId { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }
}
