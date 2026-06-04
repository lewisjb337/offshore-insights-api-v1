using System.Text.Json.Serialization;

namespace OffshoreInsights.Domain.Shared;

public class ApiResponse<T>
{
    public bool Success { get; private set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[] Errors { get; private set; } = [];    public T? Payload { get; private set; } = default;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ErrorCode { get; private set; }

    private ApiResponse() { }

    public static ApiResponse<T> Ok(T payload) => new()
    {
        Success = true,
        Payload = payload,
        Errors = []
    };

    public static ApiResponse<T> Fail(string error, int? errorCode = null) => new()
    {
        Success = false,
        Errors = [error],
        ErrorCode = errorCode
    };

    public static ApiResponse<T> Fail(string[] errors, int? errorCode = null) => new()
    {
        Success = false,
        Errors = errors,
        ErrorCode = errorCode
    };
}