using System.ComponentModel.DataAnnotations.Schema;

namespace Dumbogram.Infrasctructure.Models;

public class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedDate { get; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedDate { get; }
}