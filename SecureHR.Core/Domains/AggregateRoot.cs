using SecureHR.Core.DomainEvents;

namespace SecureHR.Core.Domains
{

    public abstract class AggregateRoot<TId>
    {
        // A private list to hold domain events.
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// The unique identifier for the Aggregate Root.
        /// </summary>
        public TId Id { get; protected set; }

        /// <summary>
        /// A read-only collection of domain events that have occurred.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adds a domain event to the aggregate. 
        /// This is meant to be called by derived classes when a state change occurs.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears the list of domain events.
        /// This is typically called by the infrastructure layer (e.g., a repository or unit of work)
        /// after the events have been dispatched.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}