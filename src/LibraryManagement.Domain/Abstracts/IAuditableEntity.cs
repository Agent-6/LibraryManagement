namespace LibraryManagement.Domain.Abstracts;

public interface IAuditableEntity 
{
    Guid CreatorId { get; set; }
    DateTime CreationTime { get; set; }

    Guid? ModifierId { get; set; }
    DateTime? ModifiedTime { get; set; }
}