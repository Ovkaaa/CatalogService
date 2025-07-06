using CatalogService.Domain.Products;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Domain.Categories;

[ExcludeFromCodeCoverage]
public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}
