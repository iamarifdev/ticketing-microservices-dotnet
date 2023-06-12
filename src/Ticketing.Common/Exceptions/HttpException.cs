using System.Runtime.Serialization;

namespace Ticketing.Common.Exceptions;

/// <summary>
/// Represents an HTTP Exception
/// </summary>
[Serializable]
public class HttpException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">The message to associate with this exception.</param>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    public HttpException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class.
    /// </summary>
    /// <param name="message">The message to associate with this exception.</param>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="innerException">The inner exception to associate with this exception</param>
    public HttpException(string message, int statusCode, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Gets the HTTP status code for this exception.
    /// </summary>
    public int StatusCode { get; }
    
    protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}