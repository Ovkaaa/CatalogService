﻿using System.Net;
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
        _serviceMock.Setup(s => s.GetAllCategoryProductsAsync(1)).ReturnsAsync(
            [
                new() { Id = 1, Name = "One" },
                new() { Id = 2, Name = "Two" }
            ]);

        // Act
        var response = await _client.GetAsync("/api/catalog/categories/1/products");

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
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var response = await _client.GetAsync("/api/catalog/products/1");

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
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(
            [
                new() { Id = 1, Name = "One" },
                new() { Id = 2, Name = "Two" }
            ]);

        // Act
        var response = await _client.GetAsync("/api/catalog/products");

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

        _serviceMock.Setup(s => s.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PostAsJsonAsync("/api/catalog/products", newProduct);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk()
    {
        // Arrange
        var update = new Product { Name = "Updated" };

        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var response = await _client.PutAsJsonAsync("/api/catalog/products/5", update);

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
        _serviceMock.Setup(s => s.DeleteAsync(3)).Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/catalog/products/3");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
