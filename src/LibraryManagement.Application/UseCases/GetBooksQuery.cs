using LibraryManagement.Application.Books;

namespace LibraryManagement.Application.UseCases;

public sealed record GetBooksQuery(BookFilterDto Filter);
