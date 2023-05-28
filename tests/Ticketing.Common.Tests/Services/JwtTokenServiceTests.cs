using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ticketing.Common.DTO;
using Ticketing.Common.Services;

namespace Ticketing.Common.Tests.Services
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private const string JwtKey = "supersecretkeythatis128bitlong";
        private const string Issuer = "example.com";
        private const string Audience = "audience";
        private const int ExpiryMinutes = 30;

        [Test]
        public void GenerateToken_ShouldGenerateValidJwtToken()
        {
            // Arrange
            var userPayload = new UserPayload("123", "test@example.com");
            var config = new JwtConfig(Issuer, Audience, ExpiryMinutes);

            // Act
            var token = JwtTokenService.GenerateToken(userPayload, config, JwtKey);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GenerateToken_ShouldGenerateJwtTokenWithCorrectClaims()
        {
            // Arrange
            var userPayload = new UserPayload("123", "test@example.com");
            var config = new JwtConfig(Issuer, Audience, ExpiryMinutes);

            // Act
            var token = JwtTokenService.GenerateToken(userPayload, config, JwtKey);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var parsedToken = handler.ReadJwtToken(token);

            parsedToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userPayload.Id);
            parsedToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == userPayload.Email);
        }
    }
}