using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CAF.Application.Abstractions.Services.Permission;
using CAF.Application.Models.Permission.VM;
using CAF.Domain.Entities.Permission;
using CAF.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.Permission
{
    public class NavigationMenuService : INavigationMenuService
    {
        private readonly CAFDbContext _context;

        public NavigationMenuService(CAFDbContext context)
        {
            _context = context;
        }

        public async Task<List<NavigationMenuViewModel>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var roleIds = await GetUserRoleIdsAsync(principal);
            var permittedMenuItems = new List<NavigationMenuViewModel>();


            foreach (var roleId in roleIds)
            {
                var roleSpecificMenus = await (from menu in _context.CAF_MENU
                                               join rolePermission in _context.CAF_PERMISSION
                                               on menu.Id equals rolePermission.MenuId
                                               where rolePermission.RoleId == roleId && menu.IsActive && !menu.IsDeleted
                                               select new NavigationMenuViewModel
                                               {
                                                   Id = menu.Id,
                                                   Name = menu.Name,
                                                   ParentId = menu.ParentId,
                                                   ControllerName = menu.ControllerName,
                                                   ActionName = menu.ActionName,
                                                   Area = menu.AreaName,
                                                   DisplayOrder = menu.DisplayOrder,
                                                   IsActive = menu.IsActive,
                                                   Icon = menu.Icon
                                               })
                           .OrderBy(m => m.DisplayOrder)
                           .ToListAsync();

                permittedMenuItems.AddRange(roleSpecificMenus);
            }

            var filteredMenus = permittedMenuItems
                .Where(menu => menu.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            return BuildMenuHierarchy(filteredMenus);
        }

        public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string? roleId)
        {
            return await (from menu in _context.CAF_MENU
                          join rolePermission in _context.CAF_PERMISSION
                          on menu.Id equals rolePermission.MenuId
                          where rolePermission.RoleId == roleId && menu.IsActive
                          select new NavigationMenuViewModel
                          {
                              Id = menu.Id,
                              Name = menu.Name,
                              ParentId = menu.ParentId,
                              ControllerName = menu.ControllerName,
                              ActionName = menu.ActionName,
                              Area = menu.AreaName,
                              Permitted = menu.Permitted,
                              DisplayOrder = menu.DisplayOrder,
                              IsActive = menu.IsActive,
                              Icon = menu.Icon
                          }).OrderBy(m => m.DisplayOrder) .ToListAsync();

        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string? controllerName, string? actionName, string? areaName)
        {
            var roleIds = await GetUserRoleIdsAsync(user);

            if (roleIds == null || !roleIds.Any())
            {
                return false;
            }

            // indekslemeyi ve sorgu 
            return await _context.CAF_PERMISSION
                .AnyAsync(permission =>
                    roleIds.Contains(permission.RoleId) &&
                    (controllerName == null || permission.Menu.ControllerName == controllerName) &&
                    (actionName == null || permission.Menu.ActionName == actionName) &&
                    (areaName == null || permission.Menu.AreaName == areaName));

        }

        public async Task<bool> SetPermissionsByRoleIdAsync(string? roleId, IEnumerable<int> permissionIds)
        {
            if (string.IsNullOrWhiteSpace(roleId) || permissionIds == null || !permissionIds.Any())
            {
                return false;
            }

            // Mevcut izinleri al
            var existingPermissions = await _context.CAF_PERMISSION
                .Where(x => x.RoleId == roleId)
                .ToListAsync();

            // Yeni izinleri belirle
            var newPermissionIds = permissionIds.ToHashSet();
            var existingPermissionIds = existingPermissions.Select(ep => ep.MenuId).ToHashSet();

            // Silinmesi gereken izinler
            var permissionsToRemove = existingPermissions
                .Where(ep => !newPermissionIds.Contains(ep.MenuId))
                .ToList();

            // Eklenecek yeni izinler
            var permissionsToAdd = newPermissionIds
                .Except(existingPermissionIds.Select(id => id))
                .Select(permissionId => new MenuRolePermission
                {
                    RoleId = roleId,
                    MenuId = int.Parse(permissionId.ToString()),
                    IsActive = true
                })
                .ToList();

            // Mevcut izinleri kaldır
            _context.CAF_PERMISSION.RemoveRange(permissionsToRemove);

            // Yeni izinleri ekle
            await _context.CAF_PERMISSION.AddRangeAsync(permissionsToAdd);

            // Değişiklikleri kaydet
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        private async Task<List<string>> GetUserRoleIdsAsync(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            return await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();
        }

        private string? GetUserId(ClaimsPrincipal user)
        {
            return ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private List<NavigationMenuViewModel> BuildMenuHierarchy(List<NavigationMenuViewModel> menuItems)
        {
            // Menülerin ID'lerini anahtar olarak kullanan bir sözlük oluştur
            var menuDictionary = menuItems.ToDictionary(m => m.Id);

            // Kök menüleri tutacak liste
            var rootMenus = new List<NavigationMenuViewModel>();

            // Her menü için
            foreach (var menu in menuItems)
            {
                // Alt menü olup olmadığını kontrol et
                if (menu.ParentId.HasValue && menuDictionary.TryGetValue(menu.ParentId.Value, out var parentMenu))
                {
                    // Eğer alt menü varsa, SubMenus listesine ekle
                    if (parentMenu.SubMenus == null)
                    {
                        parentMenu.SubMenus = new List<NavigationMenuViewModel>();
                    }
                    parentMenu.SubMenus.Add(menu);
                }
                else
                {
                    // Kök menü olarak ekle
                    rootMenus.Add(menu);
                }
            }

            return rootMenus;
        }

        


    }
}
