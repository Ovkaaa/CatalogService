using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Categories;

namespace CatalogService.API.Endpoints;

public static class CatalogEndpoints
{
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var categoriesRouteGroup = routeBuilder.MapGroup("categories").WithTags("Categories");

        categoriesRouteGroup
            .MapGet(string.Empty, async (IEntityService<Category> service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntitiesAsync(cancellationToken)))
            .WithName("GetCategories")
            .WithOpenApi();

        categoriesRouteGroup
            .MapPost(string.Empty, async (Category category, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.AddEntityAsync(category, cancellationToken);
                return Results.Created($"/api/v1/categories/{category.Id}", category);
            })
            .WithName("AddCategory")
            .WithOpenApi();

        categoriesRouteGroup
            .MapGet("{categoryId}", async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntityByIdAsync(categoryId, cancellationToken)))
            .WithName("GetCategory")
            .WithOpenApi();

        var specificCategoryRouteGroup = categoriesRouteGroup.MapGroup("{categoryId}");

        specificCategoryRouteGroup
            .MapPut(string.Empty, async (int categoryId, Category category, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                category.Id = categoryId;
                await service.UpdateEntityAsync(category, cancellationToken);
                return Results.Ok(category);
            })
            .WithName("UpdateCategory")
            .WithOpenApi();

        specificCategoryRouteGroup
            .MapDelete(string.Empty, async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.DeleteEntityAsync(categoryId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteCategory")
            .WithOpenApi();

        specificCategoryRouteGroup
            .MapGet("products", async (int categoryId, IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetProductsByCategoryIdAsync(categoryId, cancellationToken)))
            .WithName("GetProductsByCategoryId")
            .WithOpenApi()
            .WithTags("Products");
    }
}
