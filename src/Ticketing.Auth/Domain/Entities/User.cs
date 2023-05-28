using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Auth.Domain.Entities;

public class User : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(255)] public required string Email { get; set; }

    [Required] [MaxLength(255)] public required string FirstName { get; set; }

    [Required] [MaxLength(255)] public required string LastName { get; set; }

    [Required] [MaxLength(255)] public required string Password { get; set; }
}