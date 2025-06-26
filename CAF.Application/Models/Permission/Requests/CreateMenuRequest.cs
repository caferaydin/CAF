namespace CAF.Application.Models.Permission.Requests;

public class CreateMenuRequest
{
    public string? Name { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? AreaName { get; set; }
    public int DisplayOrder { get; set; }
    public bool Permitted { get; set; } = false;
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public bool IsActive { get; set; } = true;
}
