using Dumbogram.Api.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dumbogram.Api.Database.Interceptors;

public class TrackUpdatesInterceptor : SaveChangesInterceptor
{
    private void BeforeSaveChanges(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not ITrackUpdates updated) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    updated.CreatedDate = DateTimeOffset.UtcNow;
                    updated.UpdatedDate = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Modified:
                    // Rollback any changes of CreatedDate
                    context
                        .Entry(updated)
                        .Property(x => x.CreatedDate)
                        .IsModified = false;
                    updated.UpdatedDate = DateTimeOffset.UtcNow;
                    break;
            }
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