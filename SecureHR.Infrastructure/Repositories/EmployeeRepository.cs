using Microsoft.EntityFrameworkCore;
using SecureHR.Core.Domains.EmployeeAggregate;
using SecureHR.Core.Repositories;
using SecureHR.Infrastructure.Data;

namespace SecureHR.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDbContext applicationDbContext) : IEmployeeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _applicationDbContext.Employees.AddAsync(employee, cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Employees
                .Include(e => e.SalaryAdjustments)
                .Include(e => e.LeaveBookings)
                .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
        }

        public void Update(Employee employee, CancellationToken cancellationToken = default)
        {
            _applicationDbContext.Employees.Update(employee);
        }
    }
}
