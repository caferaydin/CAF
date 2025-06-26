namespace CAF.Application.Models.Permission.VM;

public class NavigationMenuViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; } 
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? Area { get; set; }
    public bool Permitted { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public string? Icon { get; set; }
    public List<NavigationMenuViewModel> SubMenus { get; set; } = new List<NavigationMenuViewModel>();
}
