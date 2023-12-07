namespace Dumbogram.Database.KeysetPagination;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int total)
    {
        AddRange(items);
        Total = total;
    }

    public int Total { get; set; }
    public string? Forward { get; set; }
    public string? Backward { get; set; }
}