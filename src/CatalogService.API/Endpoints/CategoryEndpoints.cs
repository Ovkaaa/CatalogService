using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.API.Endpoints;

public static partial class CatalogEndpoints
{
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var categoryRouteGroup = routeBuilder.MapGroup("categories").WithTags("Categories");

        categoryRouteGroup
            .MapGet("{categoryId}", async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntityByIdAsync(categoryId, cancellationToken)))
            .WithName("GetCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapGet(string.Empty, async (IEntityService<Category> service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntitiesAsync(cancellationToken)))
            .WithName("GetCategories")
            .WithOpenApi();

        categoryRouteGroup
            .MapPost(string.Empty, async (Category category, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.AddEntityAsync(category, cancellationToken);
                return Results.Created($"/api/catalog/categories/{category.Id}", category);
            })
            .WithName("AddCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapPut("{categoryId}", async (int categoryId, Category category, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                category.Id = categoryId;
                await service.UpdateEntityAsync(category, cancellationToken);
                return Results.Ok(category);
            })
            .WithName("UpdateCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapDelete("{categoryId}", async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.DeleteEntityAsync(categoryId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteCategory")
            .WithOpenApi();
    }
}
