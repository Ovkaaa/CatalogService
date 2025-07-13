using System.Diagnostics.CodeAnalysis;
using CatalogService.Domain.Categories;
using CatalogService.Domain.Products;
using CatalogService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.Entity<Category>(builder =>
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.ParentCategory)
             .WithMany(x => x.SubCategories)
             .HasForeignKey(x => x.ParentCategoryId);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Price).IsRequired().HasColumnType("money");
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Category)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId);
        });

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.EventType).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.OccurredOn).IsRequired();
        });
    }

}
