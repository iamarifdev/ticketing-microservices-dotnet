using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Persistence;
using Ticketing.Common.Extensions;

namespace Ticketing.Auth.Tests;

public class IntegrationTests : IDisposable
{
    private AuthDbContext _context = null!;
    private WebApplicationFactory<Startup> _factory = null!;

    public void Dispose()
    {
        _factory?.Dispose();
        _context?.Dispose();
    }

    [SetUp]
    public async Task SetupAsync()
    {
        _factory = CustomWebApplicationFactory<Startup>.Create();

        // Initialize test data
        using var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await _context.SeedTestData();
    }

    [Test]
    public async Task SignIn_WithValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand(TestDataHelper.TestEmail, TestDataHelper.TestPassword);

        // Act
        var response = await client.PostJsonAsync("/auth/signin", payload);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.Email.Should().Be(TestDataHelper.TestEmail);
        result.Token.Should().NotBeNullOrEmpty();
    }
    
    [Test]
    public async Task SignIn_WithInvalidEmailAddress_ReturnsValidationProblem()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand("invalid_email_address", TestDataHelper.TestPassword);

        // Act
        var response = await client.PostJsonAsync("/auth/signin", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result!.Contains("Email is not valid.").Should().BeTrue();
    }

    [Test]
    public void SignIn_WithInvalidValidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand(TestDataHelper.TestEmail, "invalid");

        // Act
        var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
        {
            var response = await client.PostJsonAsync("/auth/signin", payload);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        });

        // Assert
        exception.Should().NotBeNull();
        exception!.Message.Should().Be("Invalid credentials");
    }
}
