using SecureHR.Core.DomainEvents;

namespace SecureHR.Application.MessageBroker
{
    public interface IMessageBus
    {
        Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
    }
}
