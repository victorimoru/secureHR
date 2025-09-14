using Microsoft.EntityFrameworkCore;
using SecureHR.Core.Domains.EmployeeAggregate;
using SecureHR.Core.Domains.Idempotency;
using SecureHR.Infrastructure.Data.EntitiyConfigurations;

namespace SecureHR.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<IdempotencyKey> IdempotencyKeys { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
