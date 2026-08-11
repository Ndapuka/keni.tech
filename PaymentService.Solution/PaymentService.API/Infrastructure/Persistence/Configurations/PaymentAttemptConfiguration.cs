using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Core.Entities;

namespace PaymentService.Infrastructure.Persistence.Configurations;

public sealed class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
{
    public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
    {
        builder.ToTable("PaymentAttempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentId)
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.AttemptNumber)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ResponseCode)
            .HasMaxLength(100);

        builder.Property(x => x.ResponseMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.DurationMilliseconds)
            .IsRequired();

        builder.HasIndex(x => x.PaymentId);

        builder.HasOne<Payment>()
            .WithMany()
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}