using System.Net;
using System.Net.Http.Json;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CatalogService.API.Tests.Endpoints;

public class ProductEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IProductService> _serviceMock = new();

    private readonly HttpClient _client;

    public ProductEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => _serviceMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetAllCategoryProducts_ReturnsList()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.GetProductsByCategoryIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new() { Id = 1, Name = "One" },
                new() { Id = 2, Name = "Two" }
            ]);

        // Act
        var response = await _client.GetAsync("/api/v1/categories/1/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.Equal(2, products!.Count);
    }

    [Fact]
    public async Task GetProduct_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test" };
        _serviceMock
            .Setup(s => s.GetEntityByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var response = await _client.GetAsync("/api/v1/products/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(result);
        Assert.Equal("Test", result!.Name);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsList()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.GetEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new() { Id = 1, Name = "One" },
                new() { Id = 2, Name = "Two" }
            ]);

        // Act
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.Equal(2, products!.Count);
    }

    [Fact]
    public async Task AddProduct_ReturnsCreated()
    {
        // Arrange
        var newProduct = new Product { Id = 10, Name = "New" };

        _serviceMock
            .Setup(s => s.AddEntityAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/products", newProduct);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk()
    {
        // Arrange
        var update = new Product { Name = "Updated" };

        _serviceMock
            .Setup(s => s.UpdateEntityAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/v1/products/5", update);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Updated", result!.Name);
        Assert.Equal(5, result.Id);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOk()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.DeleteEntityAsync(3, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/v1/products/3");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
