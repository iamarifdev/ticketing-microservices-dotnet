using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Ticketing.Common.DTO;
using Ticketing.Common.Events;

namespace Ticketing.Common.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddMassTransit(
            this WebApplicationBuilder builder,
            Action<IRabbitMqReceiveEndpointConfigurator> configureEndpoint)
        {
            var config = builder.Configuration
                .GetSection(nameof(RabbitMqConfig))
                .Get<RabbitMqConfig>() ?? new RabbitMqConfig();
            
            
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context,cfg) =>
                {
                    cfg.Host(config.Host, "/", h => {
                        h.Username(config.User);
                        h.Password(config.Password);
                    });
                    cfg.ReceiveEndpoint(QueueGroup.TicketService.ToString(), configureEndpoint);
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}