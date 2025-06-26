using CAF.Domain.Entities.Authentication;

namespace CAF.Application.Models.Authentication.VM;
public class AssignRoleViewModel
{
    public List<AppUser>? Users { get; set; }
    public List<AppRole>? Roles { get; set; }
}