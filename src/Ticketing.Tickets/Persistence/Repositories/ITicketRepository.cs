using Ticketing.Tickets.Domain.Entities;

namespace Ticketing.Tickets.Persistence.Repositories;

public interface ITicketRepository
{
    Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Ticket>> GetListAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken);
}