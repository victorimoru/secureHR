using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureHR.Core.Domains.EmployeeAggregate;
using SecureHR.Core.Domains.Idempotency;
using SecureHR.Infrastructure.Data.EntitiyConfigurations;

namespace SecureHR.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        private readonly IPublisher _mediator;
        public DbSet<Employee> Employees { get; set; }
        public DbSet<IdempotencyKey> IdempotencyKeys { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker.Entries<AggregateRoot<Guid>>() 
                                            .Select(x => x.Entity.DomainEvents)
                                            .SelectMany(x => x)
                                            .ToList(); 

            ChangeTracker.Entries<AggregateRoot<Guid>>()
                         .ToList()
                         .ForEach(entry => entry.Entity.ClearDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
