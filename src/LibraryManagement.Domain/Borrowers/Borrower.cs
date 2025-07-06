using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Borrowers;

public class Borrower(string name, string email, string phoneNumber) : AuditedEntity
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;
}
