using LibraryManagement.Application.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult> GetBooks([FromQuery] BookFilterDto filter, CancellationToken cancellationToken)
    {
        var books = await _bookService.GetBooksAsync(filter, cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDto>> GetBook(int id, CancellationToken cancellationToken)
    {
        var book = await _bookService.GetBookAsync(id, cancellationToken);
        return Ok(book);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BookReadDto>> CreateBook(BookCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await _bookService.CreateBookAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetBook), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookReadDto>> UpdateBook(int id, BookUpdateDto dto, CancellationToken cancellationToken)
    {
        var updated = await _bookService.UpdateBookAsync(id, dto, cancellationToken);
        return Ok(updated);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
    {
        await _bookService.DeleteBookAsync(id, cancellationToken);
        return NoContent();
    }
}
