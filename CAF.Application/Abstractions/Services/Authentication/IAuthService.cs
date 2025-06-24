using CAF.Application.Models.Authentication.Request;

namespace CAF.Application.Abstractions.Services.Authentication;

public interface IAuthService : IInternalAuthentication
{
    Task PasswordResetAsync(PasswordResetRequest request);
    Task<bool> VerifyResetTokenAsync(ResetTokenRequest request);
}
