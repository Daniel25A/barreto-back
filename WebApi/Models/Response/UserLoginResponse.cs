namespace WebApi.Models.Response;

public class UserLoginResponse
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public long UserId { get; set; }
}