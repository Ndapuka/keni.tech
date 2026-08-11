using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Core.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ExternalTransactionId)
            .HasMaxLength(200);

        builder.Property(x => x.GatewayReference)
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => x.OrderId);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.Provider);

        builder.HasIndex(x => x.ExternalTransactionId);
    }
}
