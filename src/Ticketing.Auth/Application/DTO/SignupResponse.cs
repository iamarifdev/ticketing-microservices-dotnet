namespace Ticketing.Auth.Application.DTO;

public record SignupResponse(int Id, string Email, string FirstName, string LastName, string Token);