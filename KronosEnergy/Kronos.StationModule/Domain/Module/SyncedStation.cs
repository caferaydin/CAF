namespace Kronos.StationModule.Domain.Module;

public class SyncedStation
{
    public int Id { get; set; } // Otomatik artan ID
    public int StationCode { get; set; } 
    public string StationName { get; set; }
    public DateTime SetupDate { get; set; }
    public int Platform { get; set; }
    public string ServiceTypes { get; set; }
    public string Source { get; set; } // Örn: "Alpet", "Street", vs.
    public DateTime LastSynced { get; set; }

    public int DistributorId { get; set; } // <- yeni alan
}
