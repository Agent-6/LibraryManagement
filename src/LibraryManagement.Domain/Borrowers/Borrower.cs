using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Borrowers;

public class Borrower : AuditedEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
