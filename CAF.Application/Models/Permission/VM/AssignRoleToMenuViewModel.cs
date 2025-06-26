using CAF.Application.Models.Authentication.DTOs;
using CAF.Application.Models.Permission.DTOs;

namespace CAF.Application.Models.Permission.VM;

public class AssignRoleToMenuViewModel
{
    public List<MenuDTO> Menus { get; set; } = new List<MenuDTO>();
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
}
