using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ticketing.Common.DTO;
using Ticketing.Common.Middlewares;
using Ticketing.Common.Services;

namespace Ticketing.Common.Tests.Middlewares;

[TestFixture]
public class CurrentUserMiddlewareTests
{
    [SetUp]
    public void Setup()
    {
        _nextDelegateMock = new Mock<RequestDelegate>();
        _loggerMock = new Mock<ILogger<CurrentUserMiddleware>>();
    }

    private Mock<RequestDelegate> _nextDelegateMock = null!;
    private Mock<ILogger<CurrentUserMiddleware>> _loggerMock = null!;

    [Test]
    public async Task InvokeAsync_WithoutAuthorizationHeader_ShouldCallNextDelegate()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var middleware = new CurrentUserMiddleware(_nextDelegateMock.Object, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _nextDelegateMock.Verify(next => next(context), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithValidToken_ShouldSetUserPayloadInHttpContextItems()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var expectedUserPayload = new UserPayload(123, "test@example.com");
        var jwtConfig = new JwtConfig("example.com", "audience", 5);

        var token = JwtTokenService.GenerateToken(expectedUserPayload, jwtConfig);

        context.Request.Headers["Authorization"] = $"Bearer {token}";
        var middleware = new CurrentUserMiddleware(_nextDelegateMock.Object, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.IsTrue(context.Items.ContainsKey("User"));
        var userPayload = (UserPayload)context.Items["User"]!;
        userPayload.Should().BeEquivalentTo(expectedUserPayload);
        _nextDelegateMock.Verify(next => next(context), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WithInvalidToken_ShouldNotCallNextDelegate()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer invalid_token";
        var middleware = new CurrentUserMiddleware(_nextDelegateMock.Object, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _nextDelegateMock.Verify(next => next(context), Times.Never);
    }
}