using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(IPublishEndpoint                  publishEndPoint,
                                      IFeatureManager                   featureManager,
                                      ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{

    public async Task Handle(OrderCreatedEvent domainEvent,
                             CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}",
                              domainEvent.GetType()
                                         .Name);

        if (await featureManager.IsEnabledAsync("OrderFulfillment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

            await publishEndPoint.Publish(orderCreatedIntegrationEvent,
                                          cancellationToken);
        }
    }
}