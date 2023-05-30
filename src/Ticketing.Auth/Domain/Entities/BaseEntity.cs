using System.ComponentModel.DataAnnotations;

namespace Ticketing.Auth.Domain.Entities;

public class BaseEntity
{
    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.Now;
}