using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Borrowers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Presentation.Borrowers;

public static class BorrowersModule
{
    public static void MapBorrowersEndpoints(this IEndpointRouteBuilder app)
    {
        // Get List
        app.MapGet("api/borrowers/", async (IBorrowerService borrowerService) =>
        {
            var response = await borrowerService.GetListAsync();
            return Results.Ok(response);
        }).RequireAuthorization();

        // Get Borrower
        app.MapGet("/api/borrowers/{id}", async (Guid id, IBorrowerService borrowerService) =>
        {
            var request = new BorrowerRequest(Id: id);

            var response = await borrowerService.GetAsync(request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Create Borrower
        app.MapPost("/api/borrowers/", async (BorrowerUpdateRequest request, IBorrowerService borrowerService) =>
        {
            var response = await borrowerService.CreateAsync(request);
            if (response is null)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Update Borrower
        app.MapPut("/api/borrowers/{id}", async (Guid id, BorrowerUpdateRequest request, IBorrowerService borrowerService) =>
        {
            var response = await borrowerService.UpdateAsync(id, request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Delete Borrower
        app.MapDelete("/api/borrowers/{id}", async (Guid id, IBorrowerService borrowerService) =>
        {
            await borrowerService.DeleteAsync(id);
            return Results.Ok();
        }).RequireAuthorization();
    }
}
