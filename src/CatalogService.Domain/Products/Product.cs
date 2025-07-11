using CatalogService.Domain.Categories;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Domain.Products;

[ExcludeFromCodeCoverage]
public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Uri? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
