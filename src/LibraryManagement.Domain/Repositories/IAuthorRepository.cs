using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Repositories;

public interface IAuthorRepository
{
    Task<IReadOnlyCollection<Author>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default);

    Task UpdateAsync(Author author, CancellationToken cancellationToken = default);

    Task DeleteAsync(Author author, CancellationToken cancellationToken = default);
}
