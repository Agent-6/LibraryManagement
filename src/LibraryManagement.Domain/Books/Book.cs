using LibraryManagement.Domain.Abstracts;
using LibraryManagement.Domain.Authors;
using System.Text.Json.Serialization;

namespace LibraryManagement.Domain.Books;

public class Book : AuditedEntity
{
    public Book(string title, string iSBN, DateOnly publishDate, Author author)
    {
        Title = title;
        ISBN = iSBN;
        PublishDate = publishDate;
        AuthorId = author.Id;
    }

    [JsonConstructor]
    private Book(string title, string iSBN, DateOnly publishDate, Guid authorId)
    {
        Title = title;
        ISBN = iSBN;
        PublishDate = publishDate;
        AuthorId = authorId;
    }

    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public DateOnly PublishDate { get; set; }
    public Guid AuthorId { get; private set; }

    public void SetAuthor(Author author)
    {
        if (author is null) return;
        AuthorId = author.Id;
    }
}