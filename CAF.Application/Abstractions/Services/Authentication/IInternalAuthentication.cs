using CAF.Application.Models.Authentication.Request;
using CAF.Application.Models.Common;

namespace CAF.Application.Abstractions.Services.Authentication;

public interface IInternalAuthentication
{
    Task<ResultModel<Models.DTOs.Token>> LoginAsync(LoginRequest request);
    Task<ResultModel<Models.DTOs.Token>> RefreshTokenLoginAsync(RefreshTokenRequest request);
}
