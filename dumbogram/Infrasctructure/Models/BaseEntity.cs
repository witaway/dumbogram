namespace Dumbogram.Infrasctructure.Models;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}