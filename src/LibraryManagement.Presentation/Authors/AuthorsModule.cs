using LibraryManagement.Application.Authors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Presentation.Authors;

public static class AuthorsModule
{
    public static void MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        // Get List
        app.MapGet("api/authors/", async (IAuthorService authorService) =>
        {
            var response = await authorService.GetListAsync();
            return Results.Ok(response);
        }).RequireAuthorization();

        // Get Author
        app.MapGet("/api/authors/{id}", async (Guid id, IAuthorService authorService) =>
        {
            var request = new AuthorRequest(Id: id);

            var response = await authorService.GetAsync(request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Create Author
        app.MapPost("/api/authors/", async (AuthorUpdateRequest request, IAuthorService authorService) =>
        {
            var response = await authorService.CreateAsync(request);
            if (response is null)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Update Author
        app.MapPut("/api/authors/{id}", async (Guid id, AuthorUpdateRequest request, IAuthorService authorService) =>
        {
            var response = await authorService.UpdateAsync(id, request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Delete Author
        app.MapDelete("/api/authors/{id}", async (Guid id, IAuthorService authorService) =>
        {
            await authorService.DeleteAsync(id);
            return Results.Ok();
        }).RequireAuthorization();
    }
}
