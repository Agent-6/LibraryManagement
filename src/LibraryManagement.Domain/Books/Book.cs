using LibraryManagement.Domain.Abstracts;
using LibraryManagement.Domain.Authors;

namespace LibraryManagement.Domain.Books;

public class Book(string title, string iSBN, DateOnly publishDate, Author author) : AuditedEntity
{
    public string Title { get; set; } = title;
    public string ISBN { get; set; } = iSBN;
    public DateOnly PublishDate { get; set; } = publishDate;
    public Guid AuthorId { get; private set; } = author.Id;

    public void SetAuthor(Author author)
    {
        if (author is null) return;
        AuthorId = author.Id;
    }
}