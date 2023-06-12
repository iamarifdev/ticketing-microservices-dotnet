namespace Ticketing.Common.DTO
{
    public class ApiResponse<T> where T : class
    {
        public required int StatusCode { get; set; }
    
        public required string Message { get; set; }
    
        public required T Data { get; set; }
    
        public bool IsSuccess { get; set; } = true;
    }
}