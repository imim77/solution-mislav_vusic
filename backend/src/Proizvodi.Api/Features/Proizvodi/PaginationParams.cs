namespace Proizvodi.Api.Features.Proizvodi;

public sealed record PaginationParams(int Page, int PageSize)
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    public int Skip => (Page - 1) * PageSize;

    public static bool TryCreate(int? page, int? pageSize, out PaginationParams? paging, out string? error)
    {
        paging = null;
        error = null;

        if (page is null && pageSize is null)
        {
            return true;
        }

        var resolvedPage = page ?? DefaultPage;
        var resolvedPageSize = pageSize ?? DefaultPageSize;

        if (resolvedPage < 1)
        {
            error = $"Page must be greater than or equal to {DefaultPage}.";
            return false;
        }

        if (resolvedPageSize is < 1 or > MaxPageSize)
        {
            error = $"Page size must be between 1 and {MaxPageSize}.";
            return false;
        }

        paging = new PaginationParams(resolvedPage, resolvedPageSize);
        return true;
    }
}
