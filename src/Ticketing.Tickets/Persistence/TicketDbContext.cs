using Microsoft.EntityFrameworkCore;
using Ticketing.Tickets.Domain.Entities;

namespace Ticketing.Tickets.Persistence;

public class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; } = null!;
}