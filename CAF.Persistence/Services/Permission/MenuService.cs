using CAF.Application.Abstractions.Services.Permission;
using CAF.Application.Models.Permission.Requests;
using CAF.Application.Models.Permission.VM;
using CAF.Application.Repositories;
using CAF.Domain.Entities.Permission;
using CAF.Persistence.Contexts;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.Permission
{
    public class MenuService : IMenuService
    {
        private readonly CAFDbContext _context;
        private readonly IMapper _mapper; 
        private readonly IGenericRepository<Menu> _menuRepository;

        public MenuService(CAFDbContext context, IMapper mapper, IGenericRepository<Menu> menuRepository)
        {
            _context = context;
            _mapper = mapper;
            _menuRepository = menuRepository;
        }

        public async Task<List<Menu>> GetMenusAsync()
        {
            var result = await _menuRepository.GetWhere(x => x.IsActive && !x.IsDeleted).ToListAsync();
            return result;
        }

       
        public async Task<List<NavigationMenuViewModel>> GetPermissionsByRoleIdAsync(string roleId)
        {

            var menuItems = await (from menu in _context.CAF_MENU
                                   join rolePermission in _context.CAF_PERMISSION
                                   on menu.Id equals rolePermission.MenuId
                                   where rolePermission.RoleId == roleId && !menu.IsDeleted
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
                                   }).OrderBy(m => m.DisplayOrder)
                                     .ToListAsync();

            return menuItems;

        }

        public async Task<bool> AddAsync(Menu request)
        {
            request.IsActive = true;
            await _menuRepository.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Menu request)
        {
            var menu = await _menuRepository.GetByIdAsync(request.Id);
            if (menu == null)
                return false;

            menu.ActionName = request.ActionName ?? menu.ActionName;
            menu.ControllerName = request.ControllerName ?? menu.ControllerName;
            menu.AreaName = request.AreaName ?? menu.AreaName;
            menu.Name = request.Name ?? menu.Name;
            menu.DisplayOrder = request.DisplayOrder;
            menu.Permitted = request.Permitted;
            menu.Icon = request.Icon ?? menu.Icon;

            _menuRepository.Update(menu);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Menu> GetByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }   

        public async Task<bool> DeleteAsync(Menu item)
        {
            var menu = await _menuRepository.GetByIdAsync(item.Id);
            if (menu != null)
            {
                menu.IsDeleted = true;
                _menuRepository.Update(menu);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
