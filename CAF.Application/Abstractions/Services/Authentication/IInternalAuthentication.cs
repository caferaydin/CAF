using CAF.Application.Models.Authentication.Request;

namespace CAF.Application.Abstractions.Services.Authentication;

public interface IInternalAuthentication
{
    Task<Models.DTOs.Token> LoginAsync(LoginRequest request);
    Task<Models.DTOs.Token> RefreshTokenLoginAsync(RefreshTokenRequest request);
}
