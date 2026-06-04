using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using Mapster;

namespace LibraryManagement.Application.Genres;

public sealed class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IReadOnlyCollection<GenreReadDto>> GetGenresAsync(CancellationToken cancellationToken = default)
    {
        var genres = await _genreRepository.GetAllAsync(cancellationToken);
        return genres.Adapt<IReadOnlyCollection<GenreReadDto>>();
    }

    public async Task<GenreReadDto> GetGenreAsync(int id, CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Genre with id '{id}' was not found.");

        return genre.Adapt<GenreReadDto>();
    }

    public async Task<GenreReadDto> CreateGenreAsync(GenreCreateDto dto, CancellationToken cancellationToken = default)
    {
        var validation = new ValidationCollector();

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            validation.Add(nameof(dto.Name), "Name is required.");
        }

        validation.ThrowIfAny();

        var genre = new Genre
        {
            Name = dto.Name.Trim()
        };

        var created = await _genreRepository.AddAsync(genre, cancellationToken);
        return created.Adapt<GenreReadDto>();
    }
}
