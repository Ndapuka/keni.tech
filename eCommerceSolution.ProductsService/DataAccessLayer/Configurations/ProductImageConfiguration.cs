using BusinessLogicLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DisplayOrder)
            .IsRequired();
    }
}