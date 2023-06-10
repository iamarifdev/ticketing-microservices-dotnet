using Mapster;
using Ticketing.Tickets.Application.DTO;
using Ticketing.Tickets.Domain.Entities;

namespace Ticketing.Tickets.Application.Mapping;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Ticket, TicketResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.OrderId, src => src.OrderId);
    }
}