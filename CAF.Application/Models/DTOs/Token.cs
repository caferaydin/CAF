namespace CAF.Application.Models.DTOs;

public class Token
{
    public string? AccessToken { get; set; } // JWT Token
    public DateTime Expiration { get; set; } // Token'ın geçerlilik süresi
    public string? RefreshToken { get; set; } // Opsiyonel: Refresh Token
}
