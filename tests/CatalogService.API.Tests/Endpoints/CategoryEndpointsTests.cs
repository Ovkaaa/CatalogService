﻿using System.Net.Http.Json;
using System.Net;
using CatalogService.Application.Interfaces.Services;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

        // Act
        var response = await _client.GetAsync("/api/catalog/categories/1");

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
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Category>
        {
            new() { Id = 1, Name = "One" },
            new() { Id = 2, Name = "Two" }
        });

        // Act
        var response = await _client.GetAsync("/api/catalog/categories");

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

        _serviceMock.Setup(s => s.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsJsonAsync("/api/catalog/categories", newCategory);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsOk()
    {
        // Arrange
        var update = new Category { Name = "Updated" };

        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/catalog/categories/5", update);

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
        _serviceMock.Setup(s => s.DeleteAsync(3)).Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/catalog/categories/3");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
