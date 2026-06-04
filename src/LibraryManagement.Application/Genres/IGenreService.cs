namespace LibraryManagement.Application.Genres;

public interface IGenreService
{
    Task<IReadOnlyCollection<GenreReadDto>> GetGenresAsync(CancellationToken cancellationToken = default);

    Task<GenreReadDto> GetGenreAsync(int id, CancellationToken cancellationToken = default);

    Task<GenreReadDto> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken = default);
}
