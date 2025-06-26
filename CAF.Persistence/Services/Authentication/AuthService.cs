using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Models.Authentication.Request;
using CAF.Application.Models.DTOs;
using CAF.Application.Abstractions.Token;
using Microsoft.AspNetCore.Identity;
using MapsterMapper;
using CAF.Application.Extensions.Authentication;
using Microsoft.EntityFrameworkCore;
using CAF.Domain.Entities.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using CAF.Application.Helpers;
using CAF.Application.Abstractions.Services.ExternalService;
using CAF.Application.Models.Common;

namespace CAF.Persistence.Services.Authentication;

public class AuthService : IAuthService
{
    readonly UserManager<AppUser> _userManager;
    readonly ITokenHandler _tokenHandler;
    readonly SignInManager<AppUser> _signInManager;
    readonly IUserService _userService;
    readonly IMapper _mapper;
    readonly IMailService _mailService;

    public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService, IMapper mapper, IMailService mailService)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
        _mapper = mapper;
        _mailService = mailService;
    }

    public async Task<ResultModel<Token>> LoginAsync(LoginRequest request)
    {
        AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmailOrPhone);
        if (user == null)
            user = await _userManager.FindByEmailAsync(request.UsernameOrEmailOrPhone);

        if (user == null)
            throw new NotFoundUserException();

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded) //Authentication başarılı!
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            Token tokenResponse = await _tokenHandler.CreateAccessToken(request.AccessTokenLifeTime, user);
            var token = _mapper.Map<Token>(tokenResponse);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);

            return new()
            {
                Data = token,
                ResultCode = ResultHelpers.GetResultCode(token),
                ResultMessage = "Giriş başarılı.",
            };
        }
        throw new AuthenticationErrorException();
    }

    public async Task<bool> PasswordResetAsync(PasswordResetRequest request)
    {
        AppUser user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
            resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
            resetToken = resetToken.UrlEncode();

            await _mailService.SendPasswordResetMailAsync(request.Email, user.Id, resetToken);
            return true;
        }
        return false;
    }

    public async Task<ResultModel<Token>> RefreshTokenLoginAsync(RefreshTokenRequest request)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user != null && user?.RefreshTokenEndDate > DateTime.Now)
        {
            Token tokenResponse = await _tokenHandler.CreateAccessToken(15, user);
            var token = _mapper.Map<Token>(tokenResponse);

            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
            return new()
            {
                Data = token,
                ResultCode = ResultHelpers.GetResultCode(token),
                ResultMessage = "Token yenilendi.",
            };
        }
        else
            throw new NotFoundUserException();
    }

    public async Task<bool> VerifyResetTokenAsync(ResetTokenRequest request)
    {
        AppUser user = await _userManager.FindByIdAsync(request.UserId);
        if (user != null)
        {
            byte[] tokenBytes = WebEncoders.Base64UrlDecode(request.ResetToken);
            request.ResetToken = Encoding.UTF8.GetString(tokenBytes);
            request.ResetToken = request.ResetToken.UrlDecode();

            return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", request.ResetToken);
        }
        return false;
    }
}
