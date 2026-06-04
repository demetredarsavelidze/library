namespace LibraryManagement.Domain.Entities;

public sealed class Author
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
