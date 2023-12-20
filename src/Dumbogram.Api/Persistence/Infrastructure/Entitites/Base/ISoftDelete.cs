namespace Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedDate { get; set; }

    public void Undelete()
    {
        DeletedDate = null;
    }
}