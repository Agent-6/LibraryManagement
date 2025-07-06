namespace LibraryManagement.Application.Books;

public record BookUpdateRequest(string Title, string ISBN, DateOnly PublishDate, Guid AuthorId)
{
}