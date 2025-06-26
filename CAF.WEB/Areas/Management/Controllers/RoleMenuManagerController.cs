using System.Text.Json;
using System.Threading.Tasks;
using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Abstractions.Services.Permission;
using CAF.Application.Models.Authentication.DTOs;
using CAF.Application.Models.Permission.DTOs;
using CAF.Application.Models.Permission.Requests;
using CAF.Application.Models.Permission.VM;
using CAF.Domain.Entities.Permission;
using CAF.WEB.Extensions.Exceptions;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Areas.Management.Controllers;

[Area("Management")]
public class RoleMenuManagerController : Controller
{
    private readonly ILogger<RoleMenuManagerController> _logger;
    private readonly IMenuService _menuService;
    private readonly INavigationMenuService _navigationMenuService;
    private readonly IRoleService _roleService;

    public RoleMenuManagerController(ILogger<RoleMenuManagerController> logger, IMenuService menuService, INavigationMenuService navigationMenuService, IRoleService roleService)
    {
        _logger = logger;
        _menuService = menuService;
        _navigationMenuService = navigationMenuService;
        _roleService = roleService;
    }

    #region Role
    public async Task<IActionResult> Index()
    {
        var models = await _menuService.GetMenusAsync();

        return View(models);
    }


    [DynamicAuthorization]
    public IActionResult GetRoles([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var response = _roleService.GetAllRoles(page, size);

        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleRequest request)
    {
        var result = await _roleService.CreateRoleAsync(request.Name, request.Description);

        if (result)
        {
            TempData["SuccessMessage"] = "Rol başarıyla oluşturuldu.";
            return RedirectToAction("GetRoles");
        }

        TempData["ErrorMessage"] = "Rol oluşturulamadı. Lütfen tekrar deneyin.";
        return Redirect(Request.Headers["Referer"].ToString());

    }

    
    public async Task<IActionResult> UpdateRole(UpdateRoleRequest reqeust)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleService.UpdateRole(reqeust.Id, reqeust.Name, reqeust.Description);
            if (result)
            {
                TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rol güncellenemedi. Lütfen tekrar deneyin.";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Geçersiz model verisi.";
        }
        return Redirect(Request.Headers["Referer"].ToString());
    }

    public async Task<IActionResult> DeleteRole(string id)
    {
        
        var result = await _roleService.DeleteRole(id);
        if (result)
        {
            TempData["SuccessMessage"] = "Rol başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Rol silinemedi. Lütfen tekrar deneyin.";
        }
        return Redirect(Request.Headers["Referer"].ToString());
    }

    #endregion
    #region Menus 
    [HttpPost]
    public async Task<IActionResult> AddMenu(CreateMenuRequest createMenu)
    {
        if (ModelState.IsValid)
        {

            var menu = new Menu()
            {
                Name = createMenu.Name,
                ControllerName = createMenu.ControllerName != null ? createMenu.ControllerName : "",
                ActionName = createMenu.ActionName != null ? createMenu.ActionName : "",
                AreaName = "",
                DisplayOrder = createMenu.DisplayOrder,
                Permitted = false,
                Icon = createMenu.Icon,
                ParentId = createMenu.ParentId,
            };
            await _menuService.AddAsync(menu);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMenu(Menu updateMenu)
    {
        var existMenu = await _menuService.GetByIdAsync(updateMenu.Id);
        if (existMenu == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            await _menuService.UpdateAsync(updateMenu);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        return Redirect(Request.Headers["Referer"].ToString());

    }

    [HttpPost]
    public async Task<IActionResult> DeleteMenu(int id)
    {
        var menu = await _menuService.GetByIdAsync(id);
        if (menu == null)
        {
            return NotFound();
        }
        await _menuService.DeleteAsync(menu);

        return Redirect(Request.Headers["Referer"].ToString());
    }

    #endregion

    #region Menu Role Management
    
    public async Task<IActionResult> AssignRoleToMenu()
    {
        // Tüm rolleri al
        var roles =  _roleService.GetAllRoles(1, 100);

        // Tüm menüleri al
        var menus = await _menuService.GetMenusAsync();

        // İlk role göre menü izinlerini al (sayfa ilk yüklendiğinde)
        var rolePermissions = new List<NavigationMenuViewModel>();
        if (roles.Datas.Any())
        {
            rolePermissions = await _navigationMenuService.GetPermissionsByRoleIdAsync(roles.Datas.First().Id);
        }

        // ViewModel oluştur
        var model = new AssignRoleToMenuViewModel
        {
            Roles = roles.Datas.Select(r => new RoleDto { Id = r.Id, Name = r.Name, Description = r.Description }).ToList(),
            Menus = menus.Select(m => new MenuDTO
            {
                Id = m.Id,
                Name = m.Name,
                ControllerName = m.ControllerName,
                ActionName = m.ActionName,
                AreaName = m.AreaName,
                DisplayOrder = m.DisplayOrder,
                Permitted = rolePermissions.Any(p => p.Id == m.Id),
                Icon = m.Icon,
                ParentId = m.ParentId,
                IsActive = m.IsActive,
                CreatedDate = m.CreatedDate,
                UpdatedDate = m.LastModificationDate,
                DeletedDate = m.DeletionDate,
                SubMenus = m.SubMenus.Any() ? m.SubMenus.Select(sm => new MenuDTO
                {
                    Id = sm.Id,
                    Name = sm.Name,
                    ControllerName = sm.ControllerName,
                    ActionName = sm.ActionName,
                    AreaName = sm.AreaName,
                    DisplayOrder = sm.DisplayOrder,
                    Permitted = rolePermissions.Any(p => p.Id == sm.Id),
                    Icon = sm.Icon,
                    ParentId = sm.ParentId,
                    IsActive = sm.IsActive,
                    CreatedDate = sm.CreatedDate,
                    UpdatedDate = sm.LastModificationDate,
                    DeletedDate = sm.DeletionDate
                }).ToList() : new List<MenuDTO>()
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoleToMenuPost(string roleId, IEnumerable<int> MenuIds)
    {
        if (roleId == null)
        {
            return NotFound();
        }

        await _navigationMenuService.SetPermissionsByRoleIdAsync(roleId, MenuIds);

        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpGet("GetMenusForRole/{roleId}")]
    public async Task<JsonResult> GetMenusForRole(string roleId)
    {
        var roleMenus = await _navigationMenuService.GetPermissionsByRoleIdAsync(roleId);
        return Json(new { roleMenus });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMenus()
    {
        var menus = await _menuService.GetMenusAsync();



        var menuDtos = menus.Select(menu => new MenuDTO
        {
            Id = menu.Id,
            Name = menu.Name,
            DisplayOrder = menu.DisplayOrder,
            Icon = menu.Icon,
            ParentId = menu.ParentId,
            IsActive = menu.IsActive
        }).ToList();

        return new JsonResult(new { menus = menuDtos }, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
    #endregion
}
