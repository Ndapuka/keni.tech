using smartRestaurant.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace smartRestaurant.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<ApplicationUser> Users { get; set; } = default!;


    public DbSet<UserToken> UserTokens { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity mappings here if needed

        modelBuilder.Entity<ApplicationUser>(entity =>
        {

            entity.ToTable("Users");
            entity.HasKey(u => u.UserID);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.PersonName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();


            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.Property(u => u.UpdatedAt)
                .IsRequired(false);

            // Índice único no Email
            entity.HasIndex(u => u.Email)
                .IsUnique();
        });


        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserTokens");

            entity.HasKey(t => t.UserTokenId);

            entity.Property(t => t.Token)
                .IsRequired();

            entity.Property(t => t.TokenType)
                .HasConversion<string>();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.Property(t => t.ExpiresAt)
                .IsRequired();

            entity.Property(t => t.IsUsed)
                .IsRequired();

            entity.HasOne(t => t.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => t.Token)
                .IsUnique();
        });
    }


}





