namespace Dumbogram.Database.Pagination;

public static class PagedListQueryableExtensions
{
    public static void function(ApplicationDbContext dbContext)
    {
        var a = new MessagesPagingQuery();
//        dbContext.Files.Paginate(a => a.Id, new Guid(), PagingOrder.BEFORE, 10);
    }


    // public static PagedList<TEntity> ToPagedListAsync<TEntity>(
    //     this IQueryable<TEntity> source,
    //     PagingOptions<TEntity> pagingOptions,
    //     CancellationToken token = default) where TEntity : BaseEntity
    // {
    //     var propertySelector = pagingOptions.PropertySelector;
    //     var reference = pagingOptions.Reference;
    //     var pagingOrder = pagingOptions.PagingOrder;
    //
    //     source = source.OrderByDescending(item => propertySelector(item));
    //
    //     if (pagingOptions.PagingMode == PagingMode.First)
    //
    //     {
    //         source = pagingOrder == PagingOrder.Before
    //             ? source.Where(item => propertySelector(item).CompareTo(reference) > 0)
    //             : source.Where(item => propertySelector(item).CompareTo(reference) < 0);
    //     }
    //
    //     return new PagedList<TEntity>(source, 10);
    // }
}