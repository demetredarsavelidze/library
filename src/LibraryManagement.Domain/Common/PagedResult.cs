namespace LibraryManagement.Domain.Common;

public sealed class PagedResult<T>
{
    public PagedResult(IReadOnlyCollection<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public IReadOnlyCollection<T> Items { get; }

    public int TotalCount { get; }

    public int TotalPages { get; }

    public int CurrentPage { get; }

    public int PageSize { get; }
}
