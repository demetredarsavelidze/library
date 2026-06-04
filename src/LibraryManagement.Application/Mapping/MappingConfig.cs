using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.Genres;
using LibraryManagement.Domain.Entities;
using Mapster;

namespace LibraryManagement.Application.Mapping;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<BookCreateDto, Book>.NewConfig()
            .Map(destination => destination.Title, source => source.Title.Trim());

        TypeAdapterConfig<GenreCreateDto, Genre>.NewConfig()
            .Map(destination => destination.Name, source => source.Name.Trim());

        TypeAdapterConfig<Book, BookReadDto>.NewConfig()
            .Map(destination => destination.AuthorName, source => source.Author.FullName)
            .Map(destination => destination.GenreName, source => source.Genre.Name)
            .Map(destination => destination.BookAge, source => DateTime.UtcNow.Year - source.PublicationYear)
            .Map(destination => destination.IsThick, source => source.Pages > 100);

        TypeAdapterConfig<Author, AuthorReadDto>.NewConfig()
            .Map(destination => destination.Age,
                source => DateCalculations.CalculateAge(source.BirthDate, DateOnly.FromDateTime(DateTime.UtcNow)));
    }
}
