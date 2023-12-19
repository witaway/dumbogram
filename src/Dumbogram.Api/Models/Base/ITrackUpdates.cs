namespace Dumbogram.Api.Models.Base;

public interface ITrackUpdates
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}