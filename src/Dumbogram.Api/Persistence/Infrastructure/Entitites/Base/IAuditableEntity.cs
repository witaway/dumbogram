namespace Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

public interface IAuditableEntity : ISoftDelete, ITrackUpdates
{
}