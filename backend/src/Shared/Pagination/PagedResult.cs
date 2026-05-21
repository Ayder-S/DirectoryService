namespace Shared.Pagination;

public record PagedResult<T>(
    IReadOnlyList<T> Results,
    int Page,
    int PageSize,
    int TotalPages)
{
    public int TotalPages =>
        PageSize * (Page - 1) / PageSize; // перенос оставшихся элементов на новую неполную страницу
    
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}