using LibraryManagement.Application.Auth;
using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Genres;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
