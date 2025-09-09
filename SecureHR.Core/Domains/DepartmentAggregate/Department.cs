using SecureHR.Core.Domains.EmployeeAggregate;

namespace SecureHR.Core.Domains.DepartmentAggregate
{
    public class Department : AggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public Guid? ManagerId { get; private set; }
        public int MaxHeadcount { get; private set; }

        private readonly List<Guid> _employeeIds = [];
        public IReadOnlyCollection<Guid> EmployeeIds => _employeeIds.AsReadOnly();
        public int CurrentHeadcount => _employeeIds.Count;
        public bool HasCapacity => CurrentHeadcount < MaxHeadcount;

        private Department(Guid id, string name, int maxHeadcount)
        {
            Id = id;
            Name = name;
            MaxHeadcount = maxHeadcount;
        }
        public static Department Create(string name, int maxHeadcount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name cannot be empty.", nameof(name));
            if (maxHeadcount <= 0)
                throw new ArgumentException("Maximum headcount must be a positive number.", nameof(maxHeadcount));

            var department = new Department(Guid.NewGuid(), name, maxHeadcount);

           // department.AddDomainEvent(new DepartmentCreatedEvent(department.Id, department.Name));

            return department;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New department name cannot be empty.", nameof(newName));

            Name = newName;
        }

        public void UpdateHeadcount(int newMaxHeadcount)
        {
            if (newMaxHeadcount < CurrentHeadcount)
                throw new InvalidOperationException("New headcount cannot be less than the current number of employees.");

            MaxHeadcount = newMaxHeadcount;
        }

        public void AssignManager(Guid employeeId)
        {
            if (!_employeeIds.Contains(employeeId))
                throw new InvalidOperationException("Cannot assign a manager who is not a member of the department.");

            if (ManagerId == employeeId) return; // No change

            ManagerId = employeeId;
           // AddDomainEvent(new DepartmentManagerAssignedEvent(Id, employeeId));
        }

        public void AddEmployee(Guid employeeId)
        {
            if (!HasCapacity)
                throw new InvalidOperationException($"Department '{Name}' has reached its maximum capacity of {MaxHeadcount}.");

            if (_employeeIds.Contains(employeeId)) return;

            _employeeIds.Add(employeeId);
        }

        public void RemoveEmployee(Guid employeeId)
        {
            _employeeIds.Remove(employeeId);

            if (ManagerId == employeeId)
            {
                ManagerId = null;
            }
        }
    }

    // --- Supporting Domain Events ---
   // public record DepartmentCreatedEvent(Guid DepartmentId, string Name) : IDomainEvent;
   // public record DepartmentManagerAssignedEvent(Guid DepartmentId, Guid ManagerEmployeeId) : IDomainEvent;
}
