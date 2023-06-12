using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Auth.Application.Commands;
using Ticketing.Auth.Application.DTO;
using Ticketing.Auth.Application.Exceptions;
using Ticketing.Auth.Persistence;
using Ticketing.Common.DTO;
using Ticketing.Common.Exceptions;
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
    public async Task Signup_ExistingUser_Should_Throw_UserAlreadyExistException()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignupRequest(
            TestDataHelper.TestEmail,
            TestDataHelper.TestPassword,
            "Test",
            "User"
        );

        // Act
        var response = await client.PostJsonAsync("/auth/signup", payload);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result!.Message.Should().Be($"User is already exists by {payload.Email}");
        result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.IsSuccess.Should().Be(false);
    }
    
    [Test]
    public async Task Signup_NewUser_Should_Return_AuthResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newEmail = TestDataHelper.TestEmail.Replace("@test", "@test2");
        var payload = new SignupRequest(
            newEmail,
            TestDataHelper.TestPassword,
            "Test",
            "User"
        );
        
        // Act
        var response = await client.PostJsonAsync("/auth/signup", payload);
        response.EnsureSuccessStatusCode();
        
        // Assert
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.Email.Should().Be(newEmail);
        result.FirstName.Should().Be(payload.FirstName);
        result.LastName.Should().Be(payload.LastName);
        result.Token.Should().NotBeNull();
    }
    
    [Test]
    public async Task Signup_WithInvalid_Payload_Should_Returns_ValidationProblem()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignupRequest("", "123", "Test", "User");
        
        // Act
        var response = await client.PostJsonAsync("/auth/signup", payload);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        // Assert
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result!.Contains("Email is not valid.").Should().BeTrue();
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
    public async Task SignIn_WithInvalidEmailAddress_Returns_ValidationProblem()
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
    public async Task SignIn_WithInvalidValidCredentials_Throws_UnauthorizedException()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand(TestDataHelper.TestEmail, "invalid");

        // Act
        var response = await client.PostJsonAsync("/auth/signin", payload);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result!.Message.Should().Be("Invalid credentials");
        result.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        result.IsSuccess.Should().Be(false);
    }

    [Test]
    public async Task CurrentUser_Get_UserPayload_For_LoggedInUser()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand(TestDataHelper.TestEmail, TestDataHelper.TestPassword);

        // Act
        var signInResponse = await client.PostJsonAsync("/auth/signin", payload);
        var authResult = await signInResponse.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authResult!.Token}");
        var response = await client.GetJsonAsync("/auth/currentuser");

        // Assert
        signInResponse.EnsureSuccessStatusCode();
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<UserPayload>();
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.Email.Should().Be(TestDataHelper.TestEmail);
    }
    
    [Test]
    public async Task CurrentUser_Throws_UnauthorizedException_When_User_Not_LoggedIn()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetJsonAsync("/auth/currentuser");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Should().NotBeNull();
        result!.Message.Should().Be("Invalid credentials");
        result.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        result.IsSuccess.Should().Be(false);
    }
    
    [Test]
    public async Task SignOut_CurrentUser_Should_Expires_JWT_Cookie()
    {
        // Arrange
        var client = _factory.CreateClient();
        var payload = new SignInCommand(TestDataHelper.TestEmail, TestDataHelper.TestPassword);

        // Act
        var signInResponse = await client.PostJsonAsync("/auth/signin", payload);
        var authResult = await signInResponse.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authResult!.Token}");
        var response = await client.PostAsJsonAsync("/auth/signout", new {});

        // Assert
        response.EnsureSuccessStatusCode();
        response.Headers.TryGetValues("Set-Cookie", out var cookieValues);
        var cookies = cookieValues?.ToList();
        cookies.Should().NotBeNull();
        cookies!.Any(c => c.Contains("JWT=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/")).Should().BeTrue();
    }
}
