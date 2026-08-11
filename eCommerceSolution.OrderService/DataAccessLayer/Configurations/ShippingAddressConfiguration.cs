using BusinessLogicLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.ToTable("ShippingAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RecipientName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);
    }
}
