namespace Ticketing.Auth.Application.DTO;

public record SignupRequest(string Email, string Password, string FirstName, string LastName);