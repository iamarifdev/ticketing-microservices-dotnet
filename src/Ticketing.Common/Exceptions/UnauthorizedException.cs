using System.Net;
using System.Runtime.Serialization;

namespace Ticketing.Common.Exceptions;

[Serializable]
public class UnauthorizedException : HttpException
{
    private const int Status = (int)HttpStatusCode.Unauthorized;

    public UnauthorizedException(string message) : base(message, Status)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, Status, innerException)
    {
    }

    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}