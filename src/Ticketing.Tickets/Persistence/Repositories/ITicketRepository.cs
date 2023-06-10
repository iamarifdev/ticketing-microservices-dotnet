using Ticketing.Tickets.Domain.Entities;

namespace Ticketing.Tickets.Persistence.Repositories;

public interface ITicketRepository
{
    Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken);
}