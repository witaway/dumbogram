using System.Collections;

namespace Dumbogram.Database.Pagination;

public class PagedList<T> : IReadOnlyList<T>
{
    private readonly IList<T> subset;

    public PagedList(IEnumerable<T> items, int total)
    {
        Total = total;
        subset = items as IList<T> ?? new List<T>(items);
    }

    public int Total { get; set; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return subset.GetEnumerator();
    }

    public int Count => subset.Count;

    public T this[int index] => subset[index];

    public IEnumerator<T> GetEnumerator()
    {
        return subset.GetEnumerator();
    }
}