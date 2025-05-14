using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.API.Endpoints;

public static partial class CatalogEndpoints
{
    public static void AddProductEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var categoryProductRouteGroup = routeBuilder.MapGroup("categories/{categoryId}/products").WithTags("Categories", "Products");

        categoryProductRouteGroup
            .MapGet(string.Empty, async (int categoryId, IProductService service) => Results.Ok(await service.GetAllCategoryProductsAsync(categoryId)))
            .WithName("GetAllCategoryProducts")
            .WithOpenApi();

        var productRouteGroup = routeBuilder.MapGroup("products").WithTags("Products");

        productRouteGroup
            .MapGet("{productId}", async (int productId, IProductService service) => Results.Ok(await service.GetByIdAsync(productId)))
            .WithName("GetProduct")
            .WithOpenApi();

        productRouteGroup
            .MapGet(string.Empty, async (IProductService service) => Results.Ok(await service.GetAllAsync()))
            .WithName("GetAllProducts")
            .WithOpenApi();

        productRouteGroup
            .MapPost(string.Empty, async (Product product, IProductService service) =>
            {
                await service.AddAsync(product);
                return Results.Created($"/api/products/{product.Id}", product);
            })
            .WithName("AddProduct")
            .WithOpenApi();

        productRouteGroup
            .MapPut("{productId}", async (int productId, Product product, IProductService service) =>
            {
                product.Id = productId;
                await service.UpdateAsync(product);
                return Results.Ok(product);
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

        productRouteGroup
            .MapDelete("{productId}", async (int productId, IProductService service) =>
            {
                await service.DeleteAsync(productId);
                return Results.Ok();
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
    }
}