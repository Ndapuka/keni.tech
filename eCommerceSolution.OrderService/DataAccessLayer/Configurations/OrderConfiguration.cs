using BusinessLogicLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.SubTotal)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Discount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ShippingCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ShippingAddress)
            .WithOne(x => x.Order)
            .HasForeignKey<ShippingAddress>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}