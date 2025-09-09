using MediatR;

namespace SecureHR.Core.DomainEvents
{
    public interface IDomainEvent : INotification
    {
         DateTime OccurredOn { get; }
         string EventType { get; }
    }
}