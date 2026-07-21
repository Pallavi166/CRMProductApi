using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Xunit;

namespace API.Tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        
        var request = new LoginDto
        {
            Username = "admin",
            Password = "Admin@123"
        };

       
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        
        var request = new LoginDto
        {
            Username = "admin",
            Password = "WrongPassword"
        };

        
        var response = await _client.PostAsJsonAsync("/api/Auth/login", request);

        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_WithInvalidToken_ReturnsUnauthorized()
    {
       
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "InvalidRefreshToken"
        };

        
        var response = await _client.PostAsJsonAsync("/api/Auth/refresh-token", request);

        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}