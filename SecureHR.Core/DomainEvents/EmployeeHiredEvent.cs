using SecureHR.Core.Domains.EmployeeAggregate;

namespace SecureHR.Core.DomainEvents
{
    [Serializable]
    public record EmployeeHiredEvent(
         Guid Id,
       Guid IdempotencyKey,
       ContactInfo Contact,
       Fullname Name,
       Guid DepartmentId,
       decimal InitialSalary,
       string HireReason,
       DateTime HiredOn
      ) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string EventType => nameof(EmployeeHiredEvent);
    }
}