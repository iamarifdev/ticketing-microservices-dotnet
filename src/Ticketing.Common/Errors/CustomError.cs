namespace Ticketing.Common.Errors;

public abstract class CustomError : Exception
{
    protected CustomError(string? message = null) : base(message)
    {
        GetType().BaseType?.GetProperty("StatusCode")?.SetValue(this, StatusCode);
    }

    protected abstract int StatusCode { get; }

    public abstract (string message, string field)[] SerializeErrors();
}