using System.Diagnostics.CodeAnalysis;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            builder.Property(x => x.Price).HasColumnType("money").IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.HasOne(x => x.Category)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId);
        });
    }
}
