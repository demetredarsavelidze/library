using LibraryManagement.Application.Common;

namespace LibraryManagement.Application.Books;

public interface IBookService
{
    Task<PagedResponseDto<BookReadDto>> GetBooksAsync(BookFilterDto filter, CancellationToken cancellationToken = default);

    Task<BookReadDto> GetBookAsync(int id, CancellationToken cancellationToken = default);

    Task<BookReadDto> CreateBookAsync(BookCreateDto dto, CancellationToken cancellationToken = default);

    Task<BookReadDto> UpdateBookAsync(int id, BookUpdateDto dto, CancellationToken cancellationToken = default);

    Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
}
