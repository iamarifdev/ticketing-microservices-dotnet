namespace Ticketing.Tickets.Application.DTO;

public record TicketResponse(
    int Id, 
    string Version, 
    string Title, 
    decimal Price, 
    string UserId,
    string? OrderId
    );