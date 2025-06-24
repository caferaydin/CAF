using CAF.Domain.Entities.Authentication;

namespace CAF.Application.Abstractions.Token;

public interface ITokenHandler
{
    Task<Models.DTOs.Token> CreateAccessToken(int second, AppUser appUser);
    string CreateRefreshToken();
    Task<Models.DTOs.Token> GetUserFromTokenAsync(string token);
}
