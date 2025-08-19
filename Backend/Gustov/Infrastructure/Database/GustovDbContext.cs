using Gustov.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gustov.Infrastructure.Database
{
    public class GustovDbContext : DbContext
    {
        public GustovDbContext (DbContextOptions<GustovDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(c => c.Description)
                    .HasMaxLength(500);
                entity.Property(c => c.SortOrder)
                    .IsRequired();
                entity.Property(c => c.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);
                entity.Property(c => c.CreatedAt)
                    .IsRequired();
                entity.Property(c => c.UpdatedAt)
                    .IsRequired();

                // Relationships
                entity.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(p => p.Description)
                    .IsRequired()
                    .HasMaxLength(1000);
                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(p => p.Image)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(p => p.SortOrder)
                    .IsRequired();
                entity.Property(p => p.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);
                entity.Property(p => p.CreatedAt)
                    .IsRequired();
                entity.Property(p => p.UpdatedAt)
                    .IsRequired();
                entity.Property(p => p.CategoryId)
                    .IsRequired();

                // Relationships
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
