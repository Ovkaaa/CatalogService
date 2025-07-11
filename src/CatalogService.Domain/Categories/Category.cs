using CatalogService.Domain.Products;
using System.Diagnostics.CodeAnalysis;

namespace CatalogService.Domain.Categories;

[ExcludeFromCodeCoverage]
public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    public Uri? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; } = [];
    public ICollection<Product> Products { get; } = [];
}
