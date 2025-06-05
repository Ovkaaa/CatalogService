using CatalogService.API.Tests.Integration.Factories;
using CatalogService.Domain.Entities;
using System.Net.Http.Json;
using System.Net;

namespace CatalogService.API.Tests.Integration.Tests;

[Collection("SequentialTestCollection")]
public class ProductEndpointsTests(CustomWebApplicationFactory factory) : BaseEndpointTests(factory), IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetProduct_ReturnsProduct()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test" };
        await _client.PostAsJsonAsync("/api/v1/products", product);

        // Act
        var response = await _client.GetAsync($"/api/v1/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsList()
    {
        // Arrange
        await _client.PostAsJsonAsync("/api/v1/products", new Product { Id = 1, Name = "Test-1" });
        await _client.PostAsJsonAsync("/api/v1/products", new Product { Id = 2, Name = "Test-2" });

        // Act
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task AddProduct_ReturnsCreated()
    {
        // Arrange
        var newProduct = new Product { Id = 10, Name = "New" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/products", newProduct);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<Product>>("/api/v1/products");
        Assert.NotNull(items);
        Assert.Single(items);
        Assert.Equal(10, items[0].Id);
        Assert.Equal("New", items[0].Name);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk()
    {
        // Arrange
        var productId = 1;
        var entiity = new Product { Id = productId, Name = "Test" };
        await _client.PostAsJsonAsync("/api/v1/products", entiity);

        // Act
        entiity.Name = "Updated";
        var response = await _client.PutAsJsonAsync($"/api/v1/products/{productId}", entiity);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Updated", result.Name);

        var product = await _client.GetFromJsonAsync<Product>($"/api/v1/products/{productId}");
        Assert.NotNull(product);
        Assert.Equal(productId, product.Id);
        Assert.Equal("Updated", product.Name);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOk()
    {
        // Arrange
        var productId = 1;
        await _client.PostAsJsonAsync("/api/v1/products", new Product { Id = productId, Name = "Test-Delete" });

        // Act
        var response = await _client.DeleteAsync($"/api/v1/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<Product>>("/api/v1/products");
        Assert.NotNull(items);
        Assert.Empty(items);
    }
}
