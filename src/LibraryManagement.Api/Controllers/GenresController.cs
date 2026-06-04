using LibraryManagement.Application.Genres;
using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GenreReadDto>>> GetGenres(CancellationToken cancellationToken)
    {
        var genres = await _genreService.GetGenresAsync(cancellationToken);
        return Ok(genres);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreReadDto>> GetGenre(int id, CancellationToken cancellationToken)
    {
        var genre = await _genreService.GetGenreAsync(id, cancellationToken);
        return Ok(genre);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<GenreReadDto>> CreateGenre(GenreCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await _genreService.CreateGenreAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetGenre), new { id = created.Id }, created);
    }
}
