using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Market.Tests.ProductCategoryTests.IntegrationTests;

public class ProductCategoryIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductCategoryIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.GetAuthenticatedClientAsync().Result;
    }

    [Fact]
    public async Task Post_CreateProductCategory_ShouldReturnCreated()
    {
        // Arrange
        var request = new
        {
            Name = "Integration Test Category"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/ProductCategories", request);

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
        Assert.NotNull(result);
        Assert.True(result.ContainsKey("id"));

        var categoryId = result["id"];
        Assert.NotEqual(0, categoryId);
    }
}