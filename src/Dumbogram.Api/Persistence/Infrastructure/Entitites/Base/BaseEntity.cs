namespace Dumbogram.Api.Persistence.Infrastructure.Entitites.Base;

public abstract class BaseEntity : IAuditableEntity
{
    public DateTimeOffset? DeletedDate { get; set; } = null;
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}