using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.API.Endpoints;

public static partial class CatalogEndpoints
{
    public static void AddProductEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var categoryProductRouteGroup = routeBuilder.MapGroup("categories/{categoryId}/products").WithTags("Categories", "Products");

        categoryProductRouteGroup
            .MapGet(string.Empty, async (int categoryId, IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetProductsByCategoryIdAsync(categoryId, cancellationToken)))
            .WithName("GetProductsByCategoryId")
            .WithOpenApi();

        var productRouteGroup = routeBuilder.MapGroup("products").WithTags("Products");

        productRouteGroup
            .MapGet("{productId}", async (int productId, IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntityByIdAsync(productId, cancellationToken)))
            .WithName("GetProduct")
            .WithOpenApi();

        productRouteGroup
            .MapGet(string.Empty, async (IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntitiesAsync(cancellationToken)))
            .WithName("GetProducts")
            .WithOpenApi();

        productRouteGroup
            .MapPost(string.Empty, async (Product product, IProductService service, CancellationToken cancellationToken) =>
            {
                await service.AddEntityAsync(product, cancellationToken);
                return Results.Created($"/api/products/{product.Id}", product);
            })
            .WithName("AddProduct")
            .WithOpenApi();

        productRouteGroup
            .MapPut("{productId}", async (int productId, Product product, IProductService service, CancellationToken cancellationToken) =>
            {
                product.Id = productId;
                await service.UpdateEntityAsync(product, cancellationToken);
                return Results.Ok(product);
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

        productRouteGroup
            .MapDelete("{productId}", async (int productId, IProductService service, CancellationToken cancellationToken) =>
            {
                await service.DeleteEntityAsync(productId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
    }
}