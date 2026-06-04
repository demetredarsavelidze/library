using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Book>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        int? minPages,
        int? maxPages,
        int? genreId,
        int? authorId,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Genre)
            .AsQueryable();

        if (minPages.HasValue)
        {
            query = query.Where(book => book.Pages >= minPages.Value);
        }

        if (maxPages.HasValue)
        {
            query = query.Where(book => book.Pages <= maxPages.Value);
        }

        if (genreId.HasValue)
        {
            query = query.Where(book => book.GenreId == genreId.Value);
        }

        if (authorId.HasValue)
        {
            query = query.Where(book => book.AuthorId == authorId.Value);
        }

        query = ApplySorting(query, sortBy, sortDirection);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Book>(items, totalCount, pageNumber, pageSize);
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Books
            .Include(book => book.Author)
            .Include(book => book.Genre)
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);
    }

    public async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return book;
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Book> ApplySorting(IQueryable<Book> query, string? sortBy, string? sortDirection)
    {
        var descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy?.ToLowerInvariant() switch
        {
            "pages" => descending
                ? query.OrderByDescending(book => book.Pages)
                : query.OrderBy(book => book.Pages),
            "publicationyear" => descending
                ? query.OrderByDescending(book => book.PublicationYear)
                : query.OrderBy(book => book.PublicationYear),
            _ => descending
                ? query.OrderByDescending(book => book.Title)
                : query.OrderBy(book => book.Title)
        };
    }
}
