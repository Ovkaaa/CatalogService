using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.API.Endpoints;

public static class ProductEndpoints
{
    public static void AddProductEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var productsRouteGroup = routeBuilder.MapGroup("products").WithTags("Products");

        productsRouteGroup
            .MapGet(string.Empty, async (IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntitiesAsync(cancellationToken)))
            .WithName("GetProducts")
            .WithOpenApi();

        productsRouteGroup
            .MapPost(string.Empty, async (Product product, IProductService service, CancellationToken cancellationToken) =>
            {
                await service.AddEntityAsync(product, cancellationToken);
                return Results.Created($"/api/v1/products/{product.Id}", product);
            })
            .WithName("AddProduct")
            .WithOpenApi();

        var specificProductRouteGroup = productsRouteGroup.MapGroup("{productId}");

        specificProductRouteGroup
            .MapGet(string.Empty, async (int productId, IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntityByIdAsync(productId, cancellationToken)))
            .WithName("GetProduct")
            .WithOpenApi();

        specificProductRouteGroup
            .MapPut(string.Empty, async (int productId, Product product, IProductService service, CancellationToken cancellationToken) =>
            {
                product.Id = productId;
                await service.UpdateEntityAsync(product, cancellationToken);
                return Results.Ok(product);
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

        specificProductRouteGroup
            .MapDelete(string.Empty, async (int productId, IProductService service, CancellationToken cancellationToken) =>
            {
                await service.DeleteEntityAsync(productId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
    }
}