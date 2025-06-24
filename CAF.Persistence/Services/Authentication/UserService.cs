using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Extensions.Authentication;
using CAF.Application.Helpers;
using CAF.Application.Models;
using CAF.Application.Models.Authentication.Request;
using CAF.Application.Models.Authentication.Responses;
using CAF.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.Authentication;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest model)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Username,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            NameSurname = model.NameSurname,
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        CreateUserResponse response = new() { Succeeded = result.Succeeded };

        if (result.Succeeded)
        {
            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (roleResult.Succeeded)
            {
                response.Message = "Kullanıcı başarıyla oluşturulmuştur ve 'User' rolü atanmıştır.";
            }
            else
            {
                response.Message = "Kullanıcı oluşturuldu, ancak rol atama sırasında bir hata oluştu: ";
                foreach (var error in roleResult.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                response.Message += $"{error.Code} - {error.Description}\n";
            }
        }

        return response;
    }


    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
    {
        if (user != null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
            await _userManager.UpdateAsync(user);
        }
        else
            throw new NotFoundUserException();
    }
    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            resetToken = resetToken.UrlDecode();
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);
            else
                throw new PasswordChangeFailedException();
        }
    }
    public async Task<Pagination<GetUserResponse>> GetAllUsersAsync(int pageIndex, int pageSize)
    {
        var users = _userManager.Users.Where(x => x.EmailConfirmed == true).AsQueryable();

        var userModels = users.Select(user => new GetUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            NameSurname = user.NameSurname,
            TwoFactorEnabled = user.TwoFactorEnabled,
            UserName = user.UserName
        }).AsQueryable();

        return await Pagination<GetUserResponse>.CreateAsync(userModels, pageIndex, pageSize);


    }
    public int TotalUsersCount => _userManager.Users.Count();
    public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
    {
        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRolesAsync(user, roles);
        }
    }
    public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
    {
        AppUser user = await _userManager.FindByIdAsync(userIdOrName);
        if (user == null)
            user = await _userManager.FindByNameAsync(userIdOrName);

        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }
        return new string[] { };
    }

    public async Task<bool> IsUsernameUniqueAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user == null;
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        return user == null;
    }
}
