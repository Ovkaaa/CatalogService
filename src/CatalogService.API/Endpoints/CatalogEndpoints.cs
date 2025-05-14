namespace CatalogService.API.Endpoints;

public static partial class CatalogEndpoints
{
    public static void AddCatalogEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var catalogRouteGroup = routeBuilder.MapGroup("/api/catalog");

        catalogRouteGroup.AddCategoryEndpoints();
        catalogRouteGroup.AddProductEndpoints();
    }
}
