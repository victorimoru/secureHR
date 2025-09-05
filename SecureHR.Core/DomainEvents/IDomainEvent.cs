namespace SecureHR.Core.DomainEvents
{
    public interface IDomainEvent
    {
         DateTime OccurredOn { get; }
    }
}