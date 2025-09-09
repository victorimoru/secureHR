using SecureHR.Core.DomainEvents;

namespace SecureHR.Core.Domains.EmployeeAggregate
{
    public class Employee : AggregateRoot<Guid>
    {
        public Fullname Name { get; private set; }
        public Guid DepartmentId { get; private set; } = Guid.Empty;
        public DateTime HireDate { get; private set; }

        private readonly List<SalaryAdjustment> _salaryAdjustments = [];
        public Guid? ManagerId { get; private set; } = Guid.Empty;
        public IReadOnlyCollection<SalaryAdjustment> SalaryAdjustments => _salaryAdjustments.AsReadOnly();
        public ContactInfo PersonalContactInfo { get; private set; }
        public decimal CurrentSalary => _salaryAdjustments.OrderByDescending(s => s.EffectiveDate).FirstOrDefault()?.NewSalary ?? 0;
        public EmploymentStatus Status { get; private set; }

        private readonly List<LeaveBooking> _leaveBookings = [];
        public IReadOnlyCollection<LeaveBooking> LeaveBookings => _leaveBookings.AsReadOnly();

        private Employee() { }

        private Employee(Fullname name, Guid departmentId, DateTime hireDate, ContactInfo contactInfo)
        {
            Id = Guid.NewGuid();
            Name = name;
            DepartmentId = departmentId;
            HireDate = hireDate;
            PersonalContactInfo = contactInfo;
        }

        public static Employee Hire(Fullname name, Guid departmentId, ContactInfo contactInfo, decimal initialSalary, string reason)
        {
            if (initialSalary <= 0)
            {
                throw new ArgumentException("Initial salary must be positive.", nameof(initialSalary));
            }

            var employee = new Employee(name, departmentId, DateTime.UtcNow, contactInfo)
            {
                Id = Guid.NewGuid()
            };

            employee.AdjustSalary(initialSalary, reason);

            var newHireEvent = new EmployeeHiredEvent(
                Contact: contactInfo,
                Name: name,
                DepartmentId: departmentId,
                InitialSalary: initialSalary,
                HireReason: reason,
                HiredOn: DateTime.UtcNow
            );

            employee.AddDomainEvent(newHireEvent);
            return employee;
        }

        public void AdjustSalary(decimal newSalary, string reason)
        {
            if (newSalary < CurrentSalary)
            {
                throw new InvalidOperationException("Salary cannot be reduced.");
            }

            _salaryAdjustments.Add(new SalaryAdjustment(newSalary, reason, DateTime.UtcNow));
        }

        public void TransferToDepartment(Guid newDepartmentId)
        {
            if (newDepartmentId == Guid.Empty)
            {
                throw new ArgumentException("Department ID cannot be empty.", nameof(newDepartmentId));
            }

            if (newDepartmentId == DepartmentId)
                return; 

            var oldDepartmentId = DepartmentId;
            DepartmentId = newDepartmentId;

            //AddDomainEvent(new EmployeeTransferredEvent(Id, newDepartmentId, oldDepartmentId));
        }

        public void AssignManager(Guid managerId)
        {
            if (managerId == Id)
            {
                throw new InvalidOperationException("An employee cannot be their own manager.");
            }

            ManagerId = managerId;
        }

        public void UpdateContactInfo(ContactInfo newContactInfo)
        {
            PersonalContactInfo = newContactInfo ?? throw new ArgumentNullException(nameof(newContactInfo));
        }

        public void BookLeave(DateTime startDate, DateTime endDate)
        {
            if (Status == EmploymentStatus.Terminated)
            {
                throw new InvalidOperationException("Cannot book leave for a terminated employee.");
            }

            var newBooking = new LeaveBooking(startDate, endDate);
            _leaveBookings.Add(newBooking);
        }

        public void Terminate(string reason)
        {
            if (Status == EmploymentStatus.Terminated)
            {
                throw new InvalidOperationException("Employee is already terminated.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("A reason is required for termination.", nameof(reason));
            }

            Status = EmploymentStatus.Terminated;

            var futureBookingsToCancel = _leaveBookings
                .Where(booking => booking.StartDate > DateTime.UtcNow && !booking.IsCancelled)
                .ToList();

            foreach (var booking in futureBookingsToCancel)
            {
                booking.Cancel(); 
            }

           // AddDomainEvent(new EmployeeTerminatedEvent(this.Id, reason, DateTime.UtcNow));
        }
    }
}
