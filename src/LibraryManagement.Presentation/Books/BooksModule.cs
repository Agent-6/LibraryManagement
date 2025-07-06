using LibraryManagement.Application.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Presentation.Books;

public static class BooksModule
{
    public static void MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        // Get List
        app.MapGet("api/books/", async (IBookService bookService) =>
        {
            var response = await bookService.GetListAsync();
            return Results.Ok(response);
        }).RequireAuthorization();

        // Get Book
        app.MapGet("/api/books/{id}", async (Guid id, IBookService bookService) =>
        {
            var request = new BookRequest(Id: id);

            var response = await bookService.GetAsync(request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Create Book
        app.MapPost("/api/books/", async (BookUpdateRequest request, IBookService bookService) =>
        {
            var response = await bookService.CreateAsync(request);
            if (response is null)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Update Book
        app.MapPut("/api/books/{id}", async (Guid id, BookUpdateRequest request, IBookService bookService) =>
        {
            var response = await bookService.UpdateAsync(id, request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Delete Book
        app.MapDelete("/api/books/{id}", async (Guid id, IBookService bookService) =>
        {
            await bookService.DeleteAsync(id);
            return Results.Ok();
        }).RequireAuthorization();
    }
}
