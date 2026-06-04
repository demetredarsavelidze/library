namespace LibraryManagement.Application.Authors;

public interface IAuthorService
{
    Task<IReadOnlyCollection<AuthorReadDto>> GetAuthorsAsync(CancellationToken cancellationToken = default);

    Task<AuthorReadDto> GetAuthorAsync(int id, CancellationToken cancellationToken = default);

    Task<AuthorReadDto> CreateAuthorAsync(AuthorCreateDto dto, CancellationToken cancellationToken = default);

    Task<AuthorReadDto> UpdateAuthorAsync(int id, AuthorUpdateDto dto, CancellationToken cancellationToken = default);

    Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);
}
