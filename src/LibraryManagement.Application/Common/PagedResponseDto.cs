namespace LibraryManagement.Application.Common;

public sealed class PagedResponseDto<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public int CurrentPage { get; init; }

    public int PageSize { get; init; }
}
