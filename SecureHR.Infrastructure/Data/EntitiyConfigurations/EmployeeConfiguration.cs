using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureHR.Core.Domains.EmployeeAggregate;

namespace SecureHR.Infrastructure.Data.EntitiyConfigurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.OwnsOne(e => e.Name, fullname =>
            {
                fullname.Property(f => f.Title)
                        .HasColumnName("Title")
                        .HasMaxLength(20)
                        .IsRequired(false);

                fullname.Property(f => f.FirstName)
                        .HasColumnName("FirstName")
                        .HasMaxLength(100)
                        .IsRequired();

                fullname.Property(f => f.LastName)
                        .HasColumnName("LastName")
                        .HasMaxLength(100)
                        .IsRequired();
            });

            builder.OwnsOne(e => e.PersonalContactInfo, contact =>
            {
                contact.Property(c => c.Street)
                       .HasColumnName("Street")
                       .HasMaxLength(200);

                contact.Property(c => c.City)
                       .HasColumnName("City")
                       .HasMaxLength(100);

                contact.Property(c => c.PostalCode)
                       .HasColumnName("PostalCode")
                       .HasMaxLength(20);

                contact.Property(c => c.PhoneNumber)
                       .HasColumnName("PhoneNumber")
                       .HasMaxLength(50);

                contact.Property(c => c.Email)
                       .HasColumnName("Email")
                       .HasMaxLength(256)
                       .IsRequired(); 
            });

            builder.OwnsMany(e => e.SalaryAdjustments, salaryBuilder =>
            {
                salaryBuilder.ToTable("SalaryAdjustments"); 
                salaryBuilder.WithOwner().HasForeignKey("EmployeeId"); 
                salaryBuilder.HasKey("Id", "EmployeeId");
                salaryBuilder.Property(s => s.NewSalary).HasColumnType("decimal(18,2)");
                salaryBuilder.Property(s => s.Reason).HasMaxLength(500);
            });

            builder.OwnsMany(e => e.LeaveBookings, leaveBuilder =>
            {
                leaveBuilder.ToTable("LeaveBookings");
                leaveBuilder.WithOwner().HasForeignKey("EmployeeId");
                leaveBuilder.HasKey("Id", "EmployeeId");
            });


            // Collections — map to private backing fields
            var salaryNav = builder.Metadata.FindNavigation(nameof(Employee.SalaryAdjustments));
            salaryNav?.SetPropertyAccessMode(PropertyAccessMode.Field);

            var leaveNav = builder.Metadata.FindNavigation(nameof(Employee.LeaveBookings));
            leaveNav?.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(e => e.DepartmentId).IsRequired();
            builder.Property(e => e.ManagerId).IsRequired(false);

            builder.HasOne<Employee>()
               .WithMany()
               .HasForeignKey(e => e.ManagerId);

            builder.Property(e => e.HireDate).IsRequired();

            builder.Property(e => e.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Ignore(e => e.CurrentSalary);
        }
    }
}
