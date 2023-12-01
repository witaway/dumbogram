namespace Dumbogram.Models.Base;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedDate { get; set; }

    public void Undelete()
    {
        DeletedDate = null;
    }
}