using System.Net;
using System.Net.Http.Json;
using CatalogService.API.Tests.Integration.Factories;
using CatalogService.Domain.Categories;

namespace CatalogService.API.Tests.Integration.Tests;

[Collection("SequentialTestCollection")]
public class CategoryEndpointsTests(CustomWebApplicationFactory factory) : BaseEndpointTests(factory), IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetCategory_ReturnsCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Test" };
        await _client.PostAsJsonAsync("/api/v1/categories", category);

        // Act
        var response = await _client.GetAsync($"/api/v1/categories/{categoryId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Category>();
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsList()
    {
        // Arrange
        await _client.PostAsJsonAsync("/api/v1/categories", new Category { Id = 1, Name = "Test-1" });
        await _client.PostAsJsonAsync("/api/v1/categories", new Category { Id = 2, Name = "Test-2" });

        // Act
        var response = await _client.GetAsync("/api/v1/categories");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        Assert.NotNull(categories);
        Assert.Equal(2, categories.Count);
    }

    [Fact]
     public async Task AddCategory_ReturnsCreated()
    {
        // Arrange
        var newCategory = new Category { Id = 10, Name = "New" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/categories", newCategory);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<Category>>("/api/v1/categories");
        Assert.NotNull(items);
        Assert.Single(items);
        Assert.Equal(10, items[0].Id);
        Assert.Equal("New", items[0].Name);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsOk()
    {
        // Arrange
        var categoryId = 1;
        var entiity = new Category { Id = categoryId, Name = "Test" };
        await _client.PostAsJsonAsync("/api/v1/categories", entiity);

        // Act
        entiity.Name = "Updated";
        var response = await _client.PutAsJsonAsync($"/api/v1/categories/{categoryId}", entiity);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Category>();
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal("Updated", result.Name);

        var category = await _client.GetFromJsonAsync<Category>($"/api/v1/categories/{categoryId}");
        Assert.NotNull(category);
        Assert.Equal(categoryId, category.Id);
        Assert.Equal("Updated", category.Name);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsOk()
    {
        // Arrange
        var categoryId = 1;
        await _client.PostAsJsonAsync("/api/v1/categories", new Category { Id = categoryId, Name = "Test-Delete" });

        // Act
        var response = await _client.DeleteAsync($"/api/v1/categories/{categoryId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<Category>>("/api/v1/categories");
        Assert.NotNull(items);
        Assert.Empty(items);
    }
}
