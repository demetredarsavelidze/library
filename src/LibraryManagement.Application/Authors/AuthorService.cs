using LibraryManagement.Application.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using Mapster;

namespace LibraryManagement.Application.Authors;

public sealed class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IReadOnlyCollection<AuthorReadDto>> GetAuthorsAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.GetAllAsync(cancellationToken);
        return authors.Adapt<IReadOnlyCollection<AuthorReadDto>>();
    }

    public async Task<AuthorReadDto> GetAuthorAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Author with id '{id}' was not found.");

        return author.Adapt<AuthorReadDto>();
    }

    public async Task<AuthorReadDto> CreateAuthorAsync(AuthorCreateDto dto, CancellationToken cancellationToken = default)
    {
        ValidateAuthor(dto.FullName, dto.BirthDate);

        var author = new Author
        {
            FullName = dto.FullName.Trim(),
            BirthDate = dto.BirthDate
        };

        var created = await _authorRepository.AddAsync(author, cancellationToken);
        return created.Adapt<AuthorReadDto>();
    }

    public async Task<AuthorReadDto> UpdateAuthorAsync(int id, AuthorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Author with id '{id}' was not found.");

        ValidateAuthor(dto.FullName, dto.BirthDate);

        author.FullName = dto.FullName.Trim();
        author.BirthDate = dto.BirthDate;

        await _authorRepository.UpdateAsync(author, cancellationToken);
        return author.Adapt<AuthorReadDto>();
    }

    public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Author with id '{id}' was not found.");

        await _authorRepository.DeleteAsync(author, cancellationToken);
    }

    private static void ValidateAuthor(string fullName, DateOnly birthDate)
    {
        var validation = new ValidationCollector();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            validation.Add(nameof(AuthorCreateDto.FullName), "FullName is required.");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (birthDate > today)
        {
            validation.Add(nameof(AuthorCreateDto.BirthDate), "BirthDate cannot be in the future.");
        }
        else if (DateCalculations.CalculateAge(birthDate, today) < 18)
        {
            validation.Add(nameof(AuthorCreateDto.BirthDate), "Author must be at least 18 years old.");
        }

        validation.ThrowIfAny();
    }
}
