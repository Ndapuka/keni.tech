using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Core.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations;

public sealed class PaymentAuditConfiguration : IEntityTypeConfiguration<PaymentAudit>
{
    public void Configure(EntityTypeBuilder<PaymentAudit> builder)
    {
        builder.ToTable("PaymentAudits");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentId)
            .IsRequired();

        builder.Property(x => x.Action)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.OldStatus)
            .HasMaxLength(50);

        builder.Property(x => x.NewStatus)
            .HasMaxLength(50);

        builder.Property(x => x.PerformedBy)
            .HasMaxLength(100);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(50);

        builder.Property(x => x.UserAgent)
            .HasMaxLength(500);

        builder.Property(x => x.CorrelationId)
            .HasMaxLength(100);

        builder.HasIndex(x => x.PaymentId);

        builder.HasOne<Payment>()
            .WithMany()
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}