namespace Ticketing.Auth.Application.DTO;

public record AuthResponse(int Id, string Email, string FirstName, string LastName, string Token);