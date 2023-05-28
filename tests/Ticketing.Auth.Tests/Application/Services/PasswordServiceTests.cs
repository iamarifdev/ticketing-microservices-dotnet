using Ticketing.Auth.Application.Services;

namespace Ticketing.Auth.Tests.Application.Services;

[TestFixture]
public class PasswordServiceTests
{
    [Test]
    public async Task ToHash_ShouldReturnValidHashedPassword()
    {
        // Arrange
        const string password = "password123";

        // Act
        var hashedPassword = await PasswordService.ToHash(password);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Compare_WithValidPasswords_ShouldReturnTrue()
    {
        // Arrange
        const string password = "password123";
        var hashedPassword = await PasswordService.ToHash(password);

        // Act
        var result = await PasswordService.Compare(hashedPassword, password);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task Compare_WithInvalidPasswords_ShouldReturnFalse()
    {
        // Arrange
        const string password = "password123";
        const string invalidPassword = "wrongpassword";
        var hashedPassword = await PasswordService.ToHash(password);

        // Act
        var result = await PasswordService.Compare(hashedPassword, invalidPassword);

        // Assert
        result.Should().BeFalse();
    }
}