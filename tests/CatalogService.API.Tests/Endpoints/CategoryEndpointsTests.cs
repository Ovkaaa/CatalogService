using System.Net.Http.Json;
using System.Net;
using CatalogService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using CatalogService.Domain.Categories;

namespace CatalogService.API.Tests.Endpoints;

public class CategoryEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IEntityService<Category>> _serviceMock = new();

    private readonly HttpClient _client;

    public CategoryEndpointsTests(WebApplicationFactory<Program> factory)
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
    public async Task GetCategory_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test" };
        _serviceMock.Setup(s => s.GetEntityByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // Act
        var response = await _client.GetAsync("/api/v1/categories/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Category>();
        Assert.NotNull(result);
        Assert.Equal("Test", result!.Name);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsList()
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
        var response = await _client.GetAsync("/api/v1/categories");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        Assert.NotNull(categories);
        Assert.Equal(2, categories!.Count);
    }

    [Fact]
    public async Task AddCategory_ReturnsCreated()
    {
        // Arrange
        var newCategory = new Category { Id = 10, Name = "New" };

        _serviceMock.Setup(s => s.AddEntityAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/categories", newCategory);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsOk()
    {
        // Arrange
        var update = new Category { Name = "Updated" };

        _serviceMock.Setup(s => s.UpdateEntityAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/v1/categories/5", update);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<Category>();
        Assert.Equal("Updated", result!.Name);
        Assert.Equal(5, result.Id);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsOk()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteEntityAsync(3, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/v1/categories/3");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
