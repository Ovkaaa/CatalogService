using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;

namespace CatalogService.API.Endpoints;

public static partial class CatalogEndpoints
{
    public static void AddCategoryEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var categoryRouteGroup = routeBuilder.MapGroup("categories").WithTags("Categories");

        categoryRouteGroup
            .MapGet("{categoryId}", async (int categoryId, IEntityService<Category> service) => Results.Ok(await service.GetByIdAsync(categoryId)))
            .WithName("GetCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapGet(string.Empty, async (IEntityService<Category> service) => Results.Ok(await service.GetAllAsync()))
            .WithName("GetAllCategories")
            .WithOpenApi();

        categoryRouteGroup
            .MapPost(string.Empty, async (Category category, IEntityService<Category> service) =>
            {
                await service.AddAsync(category);
                return Results.Created($"/api/catalog/categories/{category.Id}", category);
            })
            .WithName("AddCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapPut("{categoryId}", async (int categoryId, Category category, IEntityService<Category> service) =>
            {
                category.Id = categoryId;
                await service.UpdateAsync(category);
                return Results.Ok(category);
            })
            .WithName("UpdateCategory")
            .WithOpenApi();

        categoryRouteGroup
            .MapDelete("{categoryId}", async (int categoryId, IEntityService<Category> service) =>
            {
                await service.DeleteAsync(categoryId);
                return Results.Ok();
            })
            .WithName("DeleteCategory")
            .WithOpenApi();
    }
}
