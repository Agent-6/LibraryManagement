namespace LibraryManagement.Domain.Abstracts;

public abstract class AuditedEntity(Guid? id = null) : Entity(id), IAuditableEntity
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public Guid? ModifierId { get; set; }
    public DateTime? ModifiedTime { get; set; }
}  
