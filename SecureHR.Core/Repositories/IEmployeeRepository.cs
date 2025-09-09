using SecureHR.Core.Domains.EmployeeAggregate;

namespace SecureHR.Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
        void Update(Employee employee, CancellationToken cancellationToken = default);
    }
}
