namespace Ticketing.Auth.Application.Exceptions;

public class UserAlreadyExistException : BadHttpRequestException
{
    public UserAlreadyExistException(string email): base($"User is already exists by {email}") { }
}