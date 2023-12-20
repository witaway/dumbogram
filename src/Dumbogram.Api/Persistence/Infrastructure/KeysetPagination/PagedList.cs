namespace Dumbogram.Api.Persistence.Infrastructure.KeysetPagination;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int total)
    {
        AddRange(items);
        Total = total;
    }

    public int Total { get; set; }
    public string? NextPageToken { get; set; }
    public bool CanMoveNext { get; set; } = true;
    public string? PrevPageToken { get; set; }
    public bool CanMovePrev { get; set; } = true;
}