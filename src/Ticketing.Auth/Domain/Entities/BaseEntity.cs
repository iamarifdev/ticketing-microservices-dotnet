using System.ComponentModel.DataAnnotations;

namespace Ticketing.Auth.Domain.Entities;

public class BaseEntity
{
    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}