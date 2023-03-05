using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoeUC.Core.Infrastructure.Dependency;

namespace MoeUC.Core.Infrastructure.Events;

public class EventPublisher : ISingleton
{
    public virtual async Task Publish<TEvent>(TEvent @event)
    {
        var consumers = ApplicationContext.ResolveAll<IEventConsumer<TEvent>>();
        var logger = ApplicationContext.Resolve<ILogger<EventPublisher>>();

        foreach (var consumer in consumers)
        {
            try
            {
                await consumer.HandleEvent(@event);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Handle event fail");
            }
        }
    }
}