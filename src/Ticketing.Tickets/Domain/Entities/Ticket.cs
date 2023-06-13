using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketing.Common.Entities;

namespace Ticketing.Tickets.Domain.Entities;

public class Ticket : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(255)] public required string Title { get; set; }

    [Required] public required decimal Price { get; set; }

    [Required] [MaxLength(50)] public required string UserId { get; set; }

    [Required]
    [MaxLength(10)]
    [ConcurrencyCheck]
    public byte Version { get; set; } = 1;

    [MaxLength(50)] public string? OrderId { get; set; }
}