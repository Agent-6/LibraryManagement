using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Authors;

public class Author(string name, string bio) : AuditedEntity
{
    public string Name { get; set; } = name;
    public string Bio { get; set; } = bio;
}
