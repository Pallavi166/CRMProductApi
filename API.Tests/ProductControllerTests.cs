using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.DTOs;
using Xunit;

namespace API.Tests;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task LoginAsync()
    {
        var loginDto = new LoginDto
        {
            Username = "admin",
            Password = "Admin@123"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        string token;

        if (json.TryGetProperty("AccessToken", out var accessToken))
        {
            token = accessToken.GetString()!;
        }
        else
        {
            token = json.GetProperty("accessToken").GetString()!;
        }

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
 
    }


    [Fact]
    public async Task GetAllProducts_ShouldReturnOk()
    {
       
        await LoginAsync();

        
        var response = await _client.GetAsync("/api/Products?pageNumber=1&pageSize=10");

        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
       
        await LoginAsync();

        
        var response = await _client.GetAsync("/api/Products/99999");

       
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreated()
    {
       
        await LoginAsync();

        var request = new CreateProductDto
        {
            ProductName = "Laptop",
            CreatedBy = "Integration Test"
        };

     
        var response = await _client.PostAsJsonAsync("/api/Products", request);

      
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();

        Assert.NotNull(createdProduct);
        Assert.True(createdProduct!.Id > 0);
        Assert.Equal("Laptop", createdProduct.ProductName);
    }
    [Fact]
    public async Task UpdateProduct_ShouldReturnNoContent()
    {
        
        await LoginAsync();

        var createRequest = new CreateProductDto
        {
            ProductName = "Old Product",
            CreatedBy = "Integration Test"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Products", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var product = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        var updateRequest = new UpdateProductDto
        {
            Id = product!.Id,
            ProductName = "Updated Product",
            ModifiedBy = "Integration Test"
        };

     
        var response = await _client.PutAsJsonAsync($"/api/Products/{product.Id}", updateRequest);

      
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent()
    {
      
        await LoginAsync();

        var createRequest = new CreateProductDto
        {
            ProductName = "Product To Delete",
            CreatedBy = "Integration Test"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Products", createRequest);

        createResponse.EnsureSuccessStatusCode();

        var product = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        
        var response = await _client.DeleteAsync($"/api/Products/{product!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

       
        var getResponse = await _client.GetAsync($"/api/Products/{product.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}