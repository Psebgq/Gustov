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
        public DbSet<Sale> Sales { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

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

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.SubTotal)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(s => s.TipAmount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                entity.Property(s => s.Total)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(s => s.CashRecieved)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(s => s.CashChange)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                entity.Property(s => s.CreatedAt)
                    .IsRequired();

                entity.HasMany(s => s.OrderItems)
                    .WithOne(oi => oi.Sale)
                    .HasForeignKey(oi => oi.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(oi => oi.Quantity)
                    .IsRequired();
                entity.Property(oi => oi.UnitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.TotalPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);
                entity.Property(oi => oi.CreatedAt)
                    .IsRequired();
                entity.Property(oi => oi.UpdatedAt)
                    .IsRequired();
                entity.Property(oi => oi.SaleId)
                    .IsRequired();
                entity.Property(oi => oi.CategoryId)
                    .IsRequired();

                entity.HasOne(oi => oi.Sale)
                    .WithMany(s => s.OrderItems)
                    .HasForeignKey(oi => oi.SaleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Category)
                    .WithMany()
                    .HasForeignKey(oi => oi.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
