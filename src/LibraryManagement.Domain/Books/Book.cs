using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Books;

public class Book : AuditedEntity
{
    internal Book(string title, string iSBN, DateOnly publishDate, Guid authorId)
    {
        Title = title;
        ISBN = iSBN;
        PublishDate = publishDate;
        AuthorId = authorId;
    }

    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }
    public Guid AuthorId { get; set; }
}