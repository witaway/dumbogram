namespace Dumbogram.Api.Models.Base;

public abstract class BaseEntity : ISoftDelete, ITrackUpdates
{
    public DateTimeOffset? DeletedDate { get; set; } = null;
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}