using System.ComponentModel.DataAnnotations;

namespace Ticketing.Common.Entities;

public class BaseEntity
{
    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}