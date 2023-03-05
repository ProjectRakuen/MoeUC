namespace MoeUC.Core.Infrastructure.Events;

public interface IEventConsumer<in T>
{
    Task HandleEvent(T eventMessage);
}