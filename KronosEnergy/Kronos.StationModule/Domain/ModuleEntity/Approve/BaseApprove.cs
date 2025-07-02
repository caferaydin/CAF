namespace Kronos.StationModule.Domain.ModuleEntity.Approve;

public class BaseApprove
{
    public int Id { get; set; }
    public string CreateBy { get; set; } // Oluşturan kullanıcı
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;


    public string? ApproveBy { get; set; } // Onaylayan kullanıcı
    public DateTime? ApproveDate { get; set; } = null; // Onay tarihi

    public string? RejectBy { get; set; } // Reddeden kullanıcı
    public DateTime? RejectDate { get; set; } = null; // Reddi tarihi

    public string? RejectReason { get; set; } = null; // Red nedeni

    public bool IsApproved { get; set; } = false; // Onay durumu
}
