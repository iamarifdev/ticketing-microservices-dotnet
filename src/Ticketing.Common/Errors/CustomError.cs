namespace Ticketing.Common.Errors;

public abstract class CustomError : Exception
{
    protected abstract int StatusCode { get; }

    protected CustomError(string? message = null) : base(message)
    {
        GetType().BaseType?.GetProperty("StatusCode")?.SetValue(this, StatusCode);
    }

    public abstract (string message, string field)[] SerializeErrors();
}
