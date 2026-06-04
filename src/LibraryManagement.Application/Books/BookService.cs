using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using Mapster;

namespace LibraryManagement.Application.Books;

public sealed class BookService : IBookService
{
    private static readonly HashSet<string> SortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "Title",
        "Pages",
        "PublicationYear"
    };

    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;

    public BookService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
    }

    public async Task<PagedResponseDto<BookReadDto>> GetBooksAsync(BookFilterDto filter, CancellationToken cancellationToken = default)
    {
        ValidateFilter(filter);

        var books = await _bookRepository.GetPagedAsync(
            filter.PageNumber,
            filter.PageSize,
            filter.MinPages,
            filter.MaxPages,
            filter.GenreId,
            filter.AuthorId,
            filter.SortBy,
            filter.SortDirection,
            cancellationToken);

        return new PagedResponseDto<BookReadDto>
        {
            Items = books.Items.Adapt<IReadOnlyCollection<BookReadDto>>(),
            TotalCount = books.TotalCount,
            TotalPages = books.TotalPages,
            CurrentPage = books.CurrentPage,
            PageSize = books.PageSize
        };
    }

    public async Task<BookReadDto> GetBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Book with id '{id}' was not found.");

        return book.Adapt<BookReadDto>();
    }

    public async Task<BookReadDto> CreateBookAsync(BookCreateDto dto, CancellationToken cancellationToken = default)
    {
        await ValidateBookAsync(dto.Title, dto.AuthorId, dto.GenreId, dto.Pages, dto.PublicationYear, cancellationToken);

        var book = dto.Adapt<Book>();
        var created = await _bookRepository.AddAsync(book, cancellationToken);
        var hydrated = await _bookRepository.GetByIdAsync(created.Id, cancellationToken) ?? created;

        return hydrated.Adapt<BookReadDto>();
    }

    public async Task<BookReadDto> UpdateBookAsync(int id, BookUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Book with id '{id}' was not found.");

        await ValidateBookAsync(dto.Title, dto.AuthorId, dto.GenreId, dto.Pages, dto.PublicationYear, cancellationToken);

        book.Title = dto.Title.Trim();
        book.AuthorId = dto.AuthorId;
        book.GenreId = dto.GenreId;
        book.Pages = dto.Pages;
        book.PublicationYear = dto.PublicationYear;

        await _bookRepository.UpdateAsync(book, cancellationToken);

        var hydrated = await _bookRepository.GetByIdAsync(book.Id, cancellationToken) ?? book;
        return hydrated.Adapt<BookReadDto>();
    }

    public async Task DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Book with id '{id}' was not found.");

        await _bookRepository.DeleteAsync(book, cancellationToken);
    }

    private static void ValidateFilter(BookFilterDto filter)
    {
        var validation = new ValidationCollector();

        if (filter.PageNumber < 1)
        {
            validation.Add(nameof(filter.PageNumber), "Page number must be at least 1.");
        }

        if (filter.PageSize < 1 || filter.PageSize > 100)
        {
            validation.Add(nameof(filter.PageSize), "Page size must be between 1 and 100.");
        }

        if (filter.MinPages is < 1)
        {
            validation.Add(nameof(filter.MinPages), "Minimum pages must be greater than zero.");
        }

        if (filter.MaxPages is < 1)
        {
            validation.Add(nameof(filter.MaxPages), "Maximum pages must be greater than zero.");
        }

        if (filter.MinPages.HasValue && filter.MaxPages.HasValue && filter.MinPages > filter.MaxPages)
        {
            validation.Add(nameof(filter.MinPages), "Minimum pages cannot be greater than maximum pages.");
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !SortFields.Contains(filter.SortBy))
        {
            validation.Add(nameof(filter.SortBy), "SortBy must be one of: Title, Pages, PublicationYear.");
        }

        if (!string.IsNullOrWhiteSpace(filter.SortDirection)
            && !string.Equals(filter.SortDirection, "asc", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(filter.SortDirection, "desc", StringComparison.OrdinalIgnoreCase))
        {
            validation.Add(nameof(filter.SortDirection), "SortDirection must be asc or desc.");
        }

        validation.ThrowIfAny();
    }

    private async Task ValidateBookAsync(
        string title,
        int authorId,
        int genreId,
        int pages,
        int publicationYear,
        CancellationToken cancellationToken)
    {
        var validation = new ValidationCollector();

        if (string.IsNullOrWhiteSpace(title))
        {
            validation.Add(nameof(BookCreateDto.Title), "Title is required.");
        }

        if (authorId <= 0)
        {
            validation.Add(nameof(BookCreateDto.AuthorId), "AuthorId is required.");
        }
        else if (!await _authorRepository.ExistsAsync(authorId, cancellationToken))
        {
            validation.Add(nameof(BookCreateDto.AuthorId), "Author does not exist.");
        }

        if (genreId <= 0)
        {
            validation.Add(nameof(BookCreateDto.GenreId), "GenreId is required.");
        }
        else if (!await _genreRepository.ExistsAsync(genreId, cancellationToken))
        {
            validation.Add(nameof(BookCreateDto.GenreId), "Genre does not exist.");
        }

        if (pages <= 0)
        {
            validation.Add(nameof(BookCreateDto.Pages), "Pages must be greater than zero.");
        }

        var currentYear = DateTime.UtcNow.Year;
        if (publicationYear < 1450 || publicationYear > currentYear)
        {
            validation.Add(nameof(BookCreateDto.PublicationYear), $"PublicationYear must be between 1450 and {currentYear}.");
        }

        validation.ThrowIfAny();
    }
}
