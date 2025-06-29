using CatalogService.API.Auth;
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
            .RequireAuthorization(AuthPolicy.CanRead)
            .WithOpenApi();

        categoriesRouteGroup
            .MapPost(string.Empty, async (Category category, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.AddEntityAsync(category, cancellationToken);
                return Results.Created($"/api/v1/categories/{category.Id}", category);
            })
            .WithName("AddCategory")
            .RequireAuthorization(AuthPolicy.CanCreate)
            .WithOpenApi();

        categoriesRouteGroup
            .MapGet("{categoryId}", async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetEntityByIdAsync(categoryId, cancellationToken)))
            .WithName("GetCategory")
            .RequireAuthorization(AuthPolicy.CanRead)
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
            .RequireAuthorization(AuthPolicy.CanUpdate)
            .WithOpenApi();

        specificCategoryRouteGroup
            .MapDelete(string.Empty, async (int categoryId, IEntityService<Category> service, CancellationToken cancellationToken) =>
            {
                await service.DeleteEntityAsync(categoryId, cancellationToken);
                return Results.Ok();
            })
            .WithName("DeleteCategory")
            .RequireAuthorization(AuthPolicy.CanDelete)
            .WithOpenApi();

        specificCategoryRouteGroup
            .MapGet("products", async (int categoryId, IProductService service, CancellationToken cancellationToken) =>
                Results.Ok(await service.GetProductsByCategoryIdAsync(categoryId, cancellationToken)))
            .WithName("GetProductsByCategoryId")
            .RequireAuthorization(AuthPolicy.CanRead)
            .WithOpenApi()
            .WithTags("Products");
    }
}
