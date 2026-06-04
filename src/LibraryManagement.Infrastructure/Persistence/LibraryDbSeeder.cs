using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence;

public static class LibraryDbSeeder
{
    public static async Task SeedAsync(LibraryDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (!await context.Authors.AnyAsync(cancellationToken))
        {
            var authors = new[]
            {
                new Author { FullName = "Jane Austen", BirthDate = new DateOnly(1775, 12, 16) },
                new Author { FullName = "George Orwell", BirthDate = new DateOnly(1903, 6, 25) },
                new Author { FullName = "Ursula K. Le Guin", BirthDate = new DateOnly(1929, 10, 21) }
            };

            await context.Authors.AddRangeAsync(authors, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Genres.AnyAsync(cancellationToken))
        {
            var genres = new[]
            {
                new Genre { Name = "Novel" },
                new Genre { Name = "Dystopian" },
                new Genre { Name = "Science Fiction" }
            };

            await context.Genres.AddRangeAsync(genres, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Books.AnyAsync(cancellationToken))
        {
            var authors = await context.Authors.ToListAsync(cancellationToken);
            var genres = await context.Genres.ToListAsync(cancellationToken);

            var books = new[]
            {
                new Book
                {
                    Title = "Pride and Prejudice",
                    AuthorId = authors.Single(author => author.FullName == "Jane Austen").Id,
                    GenreId = genres.Single(genre => genre.Name == "Novel").Id,
                    Pages = 279,
                    PublicationYear = 1813
                },
                new Book
                {
                    Title = "Nineteen Eighty-Four",
                    AuthorId = authors.Single(author => author.FullName == "George Orwell").Id,
                    GenreId = genres.Single(genre => genre.Name == "Dystopian").Id,
                    Pages = 328,
                    PublicationYear = 1949
                },
                new Book
                {
                    Title = "The Left Hand of Darkness",
                    AuthorId = authors.Single(author => author.FullName == "Ursula K. Le Guin").Id,
                    GenreId = genres.Single(genre => genre.Name == "Science Fiction").Id,
                    Pages = 304,
                    PublicationYear = 1969
                }
            };

            await context.Books.AddRangeAsync(books, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Users.AnyAsync(cancellationToken))
        {
            var users = new[]
            {
                new ApplicationUser
                {
                    UserName = "admin",
                    PasswordHash = PasswordHasher.Hash("Admin123!"),
                    Role = Roles.Admin
                },
                new ApplicationUser
                {
                    UserName = "reader",
                    PasswordHash = PasswordHasher.Hash("Reader123!"),
                    Role = Roles.User
                }
            };

            await context.Users.AddRangeAsync(users, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
