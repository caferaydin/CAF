using CAF.Application.Models.Permission.VM;
using CAF.Domain.Entities.Permission;

namespace CAF.Application.Abstractions.Services.Permission;

public interface IMenuService 
{
    Task<List<Menu>> GetMenusAsync();
    Task<bool> AddAsync(Menu menu);
    Task<bool> UpdateAsync(Menu request);
    Task<bool> DeleteAsync(Menu request);
    Task<Menu> GetByIdAsync(int id);
    Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string roleId);

}
