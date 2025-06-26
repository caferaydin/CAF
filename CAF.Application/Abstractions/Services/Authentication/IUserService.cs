using CAF.Application.Models.Authentication.Request;
using CAF.Application.Models.Authentication.Requests;
using CAF.Application.Models.Authentication.Responses;
using CAF.Application.Models.Common;
using CAF.Domain.Entities.Authentication;

namespace CAF.Application.Abstractions.Services.Authentication;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUserRequest model);
    Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
    Task UpdatePasswordAsync(UpdatePasswordRequest request);
    Task<Pagination<GetUserResponse>> GetAllUsersAsync(int pageIndex, int pageSize);
    int TotalUsersCount { get; }
    Task AssignRoleToUserAsnyc(string userId, string[] roles);
    Task<string[]> GetRolesToUserAsync(string userIdOrName);
    Task<bool> IsUsernameUniqueAsync(string username);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber);

}
