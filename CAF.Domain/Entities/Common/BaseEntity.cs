namespace CAF.Domain.Entities.Common;

public class BaseEntity <TKey>
{
    public TKey Id { get; set; }

    public DateTime CreationDate  { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? LastModificationDate { get; set; }
    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletionDate { get; set; }
    public string? DeletedBy { get; set; }

}