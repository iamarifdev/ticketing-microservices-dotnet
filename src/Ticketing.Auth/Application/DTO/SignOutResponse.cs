namespace Ticketing.Auth.Application.DTO;

public record SignOutResponse(bool IsSuccessful, string? ErrorMessage);