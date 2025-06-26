namespace CAF.Application.Models.Permission.Requests;

public class UpdateMenuRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? AreaName { get; set; }
    public int DisplayOrder { get; set; } = 1;
    public bool Permitted { get; set; } = true;
    public string? Icon { get; set; } // Yeni eklenen özellik
    public int? ParentId { get; set; } // Üst menü ID'si
}
