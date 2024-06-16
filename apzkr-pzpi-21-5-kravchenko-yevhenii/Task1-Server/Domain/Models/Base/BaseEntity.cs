using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public abstract class BaseEntity
{
    [Required]
    public virtual int Id { get; set; }
}
