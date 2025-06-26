namespace CAF.Domain.Entities.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? LastModificationDate { get; set; }
    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletionDate { get; set; }
    public string? DeletedBy { get; set; }
}

