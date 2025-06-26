using CAF.Domain.Entities.Common;

namespace CAF.Domain.Entities.Permission;

public class Menu : BaseEntity
{
    public string? Name { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? AreaName { get; set; }
    public int DisplayOrder { get; set; }
    public bool Permitted { get; set; }
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public virtual Menu? Parent { get; set; }
    public virtual List<Menu> SubMenus { get; set; } = new List<Menu>();
    public virtual ICollection<MenuRolePermission> MenuRolePermissions { get; set; }
    public bool IsActive { get; set; }
}
