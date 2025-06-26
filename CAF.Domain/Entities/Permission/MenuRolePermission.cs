using CAF.Domain.Entities.Common;

namespace CAF.Domain.Entities.Permission;

public class MenuRolePermission : BaseEntity
{
    public string? RoleId { get; set; }

    public int MenuId { get; set; }

    public virtual Menu? Menu { get; set; }
    public bool IsActive { get; set; } = true;
}
