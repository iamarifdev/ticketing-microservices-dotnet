using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ticketing.Common.DTO;
using Ticketing.Common.Middlewares;
using Ticketing.Common.Services;

namespace Ticketing.Common.Tests.Middlewares
{
    [TestFixture]
    public class CurrentUserMiddlewareTests
    {
        private Mock<RequestDelegate> _nextDelegateMock = null!;
        private Mock<ILogger<CurrentUserMiddleware>> _loggerMock = null!;

        [SetUp]
        public void Setup()
        {
            _nextDelegateMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<CurrentUserMiddleware>>();
        }

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
            var expectedUserPayload = new UserPayload("123", "test@example.com");
            var jwtConfig = new JwtConfig("example.com", "audience", 5);
            var jwtSecretKey = "supersecretkeythatis128bitlong";
            
            var token = JwtTokenService.GenerateToken(expectedUserPayload, jwtConfig, jwtSecretKey);
            
            context.Request.Headers["Authorization"] = $"Bearer {token}";
            var middleware = new CurrentUserMiddleware(_nextDelegateMock.Object, _loggerMock.Object);
            
            Environment.SetEnvironmentVariable("JWT_KEY", jwtSecretKey);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.IsTrue(context.Items.ContainsKey("User"));
            var userPayload = (UserPayload)context.Items["User"]!;
            userPayload.Should().BeEquivalentTo(expectedUserPayload);
            _nextDelegateMock.Verify(next => next(context), Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WithInvalidToken_ShouldLogErrorAndCallNextDelegate()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Bearer invalid_token";
            var middleware = new CurrentUserMiddleware(_nextDelegateMock.Object, _loggerMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(logger => logger.LogError("User identity claims not found"));
            _nextDelegateMock.Verify(next => next(context), Times.Once);
        }
    }
}
