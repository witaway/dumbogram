using Dumbogram.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dumbogram.Database.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private void BeforeSaveChanges(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;

            entry.State = EntityState.Modified;
            delete.DeletedDate = DateTimeOffset.UtcNow;
        }
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (eventData.Context == null) return result;
        BeforeSaveChanges(eventData.Context);
        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context == null) return result;
        BeforeSaveChanges(eventData.Context);
        return result;
    }
}