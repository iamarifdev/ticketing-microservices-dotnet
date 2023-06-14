using Microsoft.EntityFrameworkCore;
using Ticketing.Tickets.Domain.Entities;

namespace Ticketing.Tickets.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly TicketDbContext _context;

    public TicketRepository(TicketDbContext context)
    {
        _context = context;
    }
        
    public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        await _context.Tickets.AddAsync(ticket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return ticket;
    }

    public async Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken) 
        => await _context.Tickets.FindAsync(id, cancellationToken);

    public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        _context.Entry(ticket).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }
}