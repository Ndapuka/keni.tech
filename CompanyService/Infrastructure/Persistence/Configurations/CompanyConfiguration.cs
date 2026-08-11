using CompanyService.Core.Constants;
using CompanyService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyService.Infrastructure.Persistence.Configurations;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(company => company.Id);

        #region Properties

        builder.Property(company => company.Name)
            .IsRequired()
            .HasMaxLength(CompanyConstants.NameMaxLength);

        builder.Property(company => company.Slug)
            .HasMaxLength(CompanyConstants.SlugMaxLength);

        builder.HasIndex(company => company.Slug)
            .IsUnique()
            .HasFilter("[Slug] IS NOT NULL AND [Slug] <> ''");

        builder.Property(company => company.Description)
            .HasMaxLength(CompanyConstants.DescriptionMaxLength);

        builder.Property(company => company.TaxNumber)
            .HasMaxLength(CompanyConstants.TaxNumberLength);

        builder.HasIndex(company => company.TaxNumber)
            .IsUnique()
            .HasFilter("[TaxNumber] IS NOT NULL AND [TaxNumber] <> ''");

        builder.Property(company => company.LogoUrl)
            .HasMaxLength(CompanyConstants.LogoUrlMaxLength);

        builder.Property(company => company.OwnerUserId)
            .IsRequired();

        builder.Property(company => company.BusinessType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(company => company.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(company => company.WizardStep)
            .HasConversion<int>()
            .IsRequired();

        #endregion

        #region Contact

        builder.OwnsOne(company => company.Contact, contact =>
        {
            contact.Property(c => c.Email)
                .HasColumnName("Email")
                .HasMaxLength(CompanyConstants.EmailMaxLength);

            contact.Property(c => c.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(CompanyConstants.PhoneMaxLength);

            contact.HasIndex(c => c.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL AND [Email] <> ''");
        });

        #endregion

        #region Address

        builder.OwnsOne(company => company.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("Street")
                .HasMaxLength(CompanyConstants.AddressMaxLength);

            address.Property(a => a.City)
                .HasColumnName("City")
                .HasMaxLength(CompanyConstants.CityMaxLength)
                .IsRequired();

            address.Property(a => a.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(CompanyConstants.PostalCodeMaxLength);

            address.Property(a => a.Country)
                .HasColumnName("Country")
                .HasMaxLength(CompanyConstants.CountryMaxLength)
                .IsRequired();
        });

        #endregion

        #region Users

        builder.HasMany(company => company.Users)
            .WithOne()
            .HasForeignKey(user => user.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(company => company.Users)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        #endregion
    }
}