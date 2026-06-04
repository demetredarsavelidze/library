using LibraryManagement.Application.Abstractions.Authentication;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Repositories;
using LibraryManagement.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseInMemoryDatabase(configuration.GetConnectionString("LibraryDb") ?? "LibraryDb"));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
