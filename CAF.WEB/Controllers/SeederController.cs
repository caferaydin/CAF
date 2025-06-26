using System.Threading.Tasks;
using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Abstractions.Services.Permission;
using CAF.Application.Models.Authentication.DTOs;
using CAF.Application.Repositories;
using CAF.Domain.Entities.Authentication;
using CAF.Domain.Entities.Permission;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CAF.WEB.Controllers
{
    public class SeederController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly INavigationMenuService _navigationMenuService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IGenericRepository<Menu> _menuRepository;

        public SeederController(IUserService userService, IRoleService roleService, IMenuService menuService, INavigationMenuService navigationMenuService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IGenericRepository<Menu> menuRepository)
        {
            _userService = userService;
            _roleService = roleService;
            _menuService = menuService;
            _navigationMenuService = navigationMenuService;
            _userManager = userManager;
            _roleManager = roleManager;
            _menuRepository = menuRepository;
        }

        public async Task<IActionResult> Index()
        {
            await CreateMenus();
            await UserData();

            return View();
        }

        public async Task<bool> CreateMenus()
        {
            var menus = new List<Menu>();

            var dashBoardMenu = new Menu
            {
                Name = "Dashboard",
                ControllerName = "Home",
                ActionName = "Index",
                AreaName = "",
                DisplayOrder = 1,
                Permitted = true,
                IsActive = true,
                Icon = "fa fa-home"
            };
            menus.Add(dashBoardMenu);

            var management = new Menu
            {
                Name = "Management",
                ControllerName = "",
                ActionName = "",
                AreaName = "",
                DisplayOrder = 10,
                Permitted = true,
                IsActive = true,
                Icon = "fa fa-cog",

                SubMenus = new()
                {
                    new Menu
                    {
                        Name = "User Manager",
                        ControllerName = "",
                        ActionName = "",
                        AreaName = "",
                        DisplayOrder = 1,
                        Permitted = true,
                        IsActive = true,
                        Icon = "fa fa-users",
                        SubMenus = new()
                        {
                            new Menu
                            {
                                Name = "Get All Users",
                                ControllerName = "UserManager",
                                ActionName = "GetAllUsers",
                                AreaName = "Management",
                                DisplayOrder = 1,
                                Permitted = true,
                                IsActive = true,
                                Icon = "fa fa-list"
                            },
                            new Menu
                            {
                                Name = "Assign Role To User",
                                ControllerName = "UserManager",
                                ActionName = "AssignRoleToUser",
                                AreaName = "Management",
                                DisplayOrder = 2,
                                Permitted = true,
                                IsActive = true,
                                Icon = "fa fa-user-plus"
                            }
                        }
                        
                    },
                    new Menu
                    {
                        Name = "Role Manager",
                        ControllerName = "",
                        ActionName = "",
                        AreaName = "",
                        DisplayOrder = 2,
                        Permitted = true,
                        IsActive = true,
                        Icon = "fa fa-user-tag",
                        SubMenus = new()
                        {
                            new Menu
                            {
                                Name = "Roles",
                                ControllerName = "RoleMenuManager",
                                ActionName = "GetRoles",
                                AreaName = "Management",
                                DisplayOrder = 1,
                                Permitted = true,
                                IsActive = true,
                                Icon = "fa fa-list"
                            },
                            new Menu
                            {
                                Name = "Menus",
                                ControllerName = "RoleMenuManager",
                                ActionName = "Index",
                                AreaName = "Management",
                                DisplayOrder = 2,
                                Permitted = true,
                                IsActive = true,
                                Icon = "fa fa-plus"
                            },
                            new Menu
                            {
                                Name = "Assign Role To Menu",
                                ControllerName = "RoleMenuManager",
                                ActionName = "AssignRoleToMenu",
                                AreaName = "Management",
                                DisplayOrder = 2,
                                Permitted = true,
                                IsActive = true,
                                Icon = "fa fa-plus"
                            }
                        }
                    }
                }
            };

            menus.Add(management);

            await _menuRepository.AddRangeAsync(menus);
            await _menuRepository.SaveAsync();

            return true;
        }

        public async Task<bool> UserData()
        {
            await CreateUser();
            await CreateRole();
            await UserRoleAssign();
            await RoleToMenuAssign();

            return true;
        }



        #region UserData

        public async Task<bool> CreateUser()
        {
            AppUser user = new()
            {
                Email = "zng.caferaydin@gmail.com",
                NameSurname = "Cafer AYDIN",
                PhoneNumber = "5421092933",
                UserName = "ccaferaydin",
                BirthDate = DateTime.ParseExact("27-12-1994", "dd-MM-yyyy", null),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            await _userManager.CreateAsync(user, "123456ada");

            return true;
        }

        public async Task<bool> CreateRole()
        {
            await _roleService.CreateRoleAsync("Admin", "Admin");

            return true;
        }

        public async Task<bool> UserRoleAssign()
        {
            var users = await _userService.GetAllUsersAsync(1, 10);

            foreach (var item in users)
            {
                await _userService.AssignRoleToUserAsnyc(item.Id, ["Admin"]);

            }

            return true;
        }

        public async Task<bool> RoleToMenuAssign()
        {
            //var menus =   _menuService.GetAll().AsEnumerable().Select(x => x.Id);
            var menus = await _menuService.GetMenusAsync();
            var menuIds = menus.Select(x => x.Id).ToList();


            var query = _roleManager.Roles;

            IQueryable<AppRole> rolesQuery = null;

            rolesQuery = query;

            var roles = rolesQuery
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name, Description = r.Description })
                .FirstOrDefault();

            var roleId = roles?.Id;

            await _navigationMenuService.SetPermissionsByRoleIdAsync(roleId, menuIds);

            return true;
        }

        #endregion



    }
}
