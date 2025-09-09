using SecureHR.Core.DomainEvents;

namespace SecureHR.Core.Domains.EmployeeAggregate
{
    public abstract class AggregateRoot<T>
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        public T Id { get; protected set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}