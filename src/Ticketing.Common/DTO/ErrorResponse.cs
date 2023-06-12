namespace Ticketing.Common.DTO;

public class ErrorResponse
{
    public required int StatusCode { get; set; }
    public required string Message { get; set; }
    
    public bool IsSuccess { get; set; } = false;
}