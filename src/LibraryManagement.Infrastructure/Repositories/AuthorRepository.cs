using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .AsNoTracking()
            .OrderBy(author => author.FullName)
            .ToListAsync(cancellationToken);
    }

    public Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Authors.FirstOrDefaultAsync(author => author.Id == id, cancellationToken);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Authors.AnyAsync(author => author.Id == id, cancellationToken);
    }

    public async Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        await _context.Authors.AddAsync(author, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return author;
    }

    public async Task UpdateAsync(Author author, CancellationToken cancellationToken = default)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Author author, CancellationToken cancellationToken = default)
    {
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
