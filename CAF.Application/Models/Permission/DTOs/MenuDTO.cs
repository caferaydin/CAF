namespace CAF.Application.Models.Permission.DTOs;

public class MenuDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? AreaName { get; set; }
    public int DisplayOrder { get; set; }
    public bool Permitted { get; set; }
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public MenuDTO? Parent { get; set; }
    public List<MenuDTO> SubMenus { get; set; } = new List<MenuDTO>();
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}
