using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Repositories;

public interface IGenreRepository
{
    Task<IReadOnlyCollection<Genre>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken = default);
}
