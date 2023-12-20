namespace Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

public interface ITrackUpdates
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}