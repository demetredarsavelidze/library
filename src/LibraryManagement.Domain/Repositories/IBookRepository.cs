using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Repositories;

public interface IBookRepository
{
    Task<PagedResult<Book>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        int? minPages,
        int? maxPages,
        int? genreId,
        int? authorId,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);

    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);

    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
}
