namespace Kronos.StationModule.Model.Requests;

public class StationPagedRequest
{
    public int Start { get; set; }         // kaçıncı satırdan başlanacak
    public int Length { get; set; }        // kaç kayıt alınacak
    public string? Search { get; set; }    // filtre araması

    // Gelişmiş filtre alanları istersen:
    public string? StationName { get; set; }
    public DateTime? MinSetupDate { get; set; }
    public DateTime? MaxSetupDate { get; set; }
}
