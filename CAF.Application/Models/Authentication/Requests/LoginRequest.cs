namespace CAF.Application.Models.Authentication.Request;

public class LoginRequest
{
    public string UsernameOrEmailOrPhone { get; set; }
    public string Password { get; set; }
    public int AccessTokenLifeTime { get; set; } = 30; // Default 30 minute
}
