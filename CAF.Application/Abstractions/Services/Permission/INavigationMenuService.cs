using System.Security.Claims;
using CAF.Application.Models.Permission.VM;

namespace CAF.Application.Abstractions.Services.Permission;

public interface INavigationMenuService
{
    Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal);
    Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string? id);
    Task<bool> HasPermissionAsync(ClaimsPrincipal user, string? controllerName, string? actionName, string? areaName);
    Task<bool> SetPermissionsByRoleIdAsync(string? id, IEnumerable<int> permissionIds);
}
