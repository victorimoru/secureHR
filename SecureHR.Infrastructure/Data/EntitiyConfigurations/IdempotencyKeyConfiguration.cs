using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureHR.Core.Domains.Idempotency;

namespace SecureHR.Infrastructure.Data.EntitiyConfigurations
{
    internal class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
    {
        public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
        {
            builder.ToTable("IdempotencyKeys");

            builder.HasKey(ik => ik.Id);
            builder.Property(ik => ik.Id).ValueGeneratedNever();

            builder.Property(ik => ik.RequestName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ik => ik.CreatedAtUtc)
                .IsRequired();
        }
    }
}
