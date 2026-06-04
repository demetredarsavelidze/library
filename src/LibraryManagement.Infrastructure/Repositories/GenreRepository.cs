using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public sealed class GenreRepository : IGenreRepository
{
    private readonly LibraryDbContext _context;

    public GenreRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Genres
            .AsNoTracking()
            .OrderBy(genre => genre.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Genres.FirstOrDefaultAsync(genre => genre.Id == id, cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Genres.AnyAsync(genre => genre.Id == id, cancellationToken);
    }

    public async Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken = default)
    {
        await _context.Genres.AddAsync(genre, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return genre;
    }
}
