using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Ticketing.Common.DTO;
using Ticketing.Common.Events;

namespace Ticketing.Common.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddServiceBus(
            this WebApplicationBuilder builder,
            Action<WebApplicationBuilder> registerPublishers,
            Action<IRabbitMqReceiveEndpointConfigurator> registerListeners)
        {
            var config = builder.Configuration
                .GetSection(nameof(RabbitMqConfig))
                .Get<RabbitMqConfig>() ?? new RabbitMqConfig();

            registerPublishers(builder);
            
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context,cfg) =>
                {
                    cfg.Host(config.Host, "/", h => {
                        h.Username(config.User);
                        h.Password(config.Password);
                    });
                    cfg.ReceiveEndpoint(QueueGroup.TicketService.ToString(), registerListeners);
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}