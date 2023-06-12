using System.Runtime.Serialization;
using Ticketing.Common.Exceptions;

namespace Ticketing.Auth.Application.Exceptions;

[Serializable]
public class UserAlreadyExistException : BadRequestException
{
    public UserAlreadyExistException(string email): base($"User is already exists by {email}") { }
    

    protected UserAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}