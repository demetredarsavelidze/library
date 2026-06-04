using LibraryManagement.Application.Authors;
using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AuthorReadDto>>> GetAuthors(CancellationToken cancellationToken)
    {
        var authors = await _authorService.GetAuthorsAsync(cancellationToken);
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id, CancellationToken cancellationToken)
    {
        var author = await _authorService.GetAuthorAsync(id, cancellationToken);
        return Ok(author);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<AuthorReadDto>> CreateAuthor(AuthorCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await _authorService.CreateAuthorAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAuthor), new { id = created.Id }, created);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuthorReadDto>> UpdateAuthor(
        int id,
        AuthorUpdateDto dto,
        CancellationToken cancellationToken)
    {
        var updated = await _authorService.UpdateAuthorAsync(id, dto, cancellationToken);
        return Ok(updated);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
    {
        await _authorService.DeleteAuthorAsync(id, cancellationToken);
        return NoContent();
    }
}
