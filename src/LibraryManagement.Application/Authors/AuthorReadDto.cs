namespace LibraryManagement.Application.Authors;

public sealed class AuthorReadDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }

    public int Age { get; set; }
}
