using BusinessLogicLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PromotionalPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.PreparationTimeHours)
            .IsRequired();

        builder.Property(x => x.MinimumAdvanceHours)
            .IsRequired();

        builder.Property(x => x.MaximumDailyQuantity)
            .IsRequired();

        builder.Property(x => x.StockControlled)
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .IsRequired();

        builder.Property(x => x.IsFeatured)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasMany(x => x.Images)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}