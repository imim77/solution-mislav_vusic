namespace Proizvodi.Api.Features.Proizvodi;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}

public static class PagedResult
{
    public static PagedResult<T> Create<T>(IReadOnlyList<T> source, int page, int pageSize)
    {
        var items = source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<T>(items, source.Count, page, pageSize);
    }
}
