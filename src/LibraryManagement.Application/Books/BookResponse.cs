namespace LibraryManagement.Application.Books;

public record BookResponse(Guid Id, string Title, string ISBN, DateOnly PublishDate, Guid AuthorId)
{
}