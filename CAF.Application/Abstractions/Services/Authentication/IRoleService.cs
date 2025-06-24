using CAF.Application.Models;
using CAF.Application.Models.Authentication.DTOs;

namespace CAF.Application.Abstractions.Services.Authentication;

public interface IRoleService
{
    Pagination<RoleDto> GetAllRoles(int page, int size);
    Task<(string id, string name)> GetRoleById(string id);
    Task<bool> CreateRole(string name, string description);
    Task<bool> DeleteRole(string id);
    Task<bool> UpdateRole(string id, string name);
}
