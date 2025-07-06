using LibraryManagement.Application.Loans;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraryManagement.Presentation.Loans;

public static class LoansModule
{
    public static void MapLoansEndpoints(this IEndpointRouteBuilder app)
    {
        // Get List
        app.MapGet("api/loans/", async (ILoanService loanService) =>
        {
            var response = await loanService.GetListAsync();
            return Results.Ok(response);
        }).RequireAuthorization();

        // Create Loan 
        app.MapPost("/api/loans/", async (LoanCreateRequest request, ILoanService loanService) =>
        {
            var response = await loanService.CreateAsync(request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Update Loan
        app.MapPut("/api/loans/{id}", async (Guid id, ILoanService loanService) =>
        {
            var response = await loanService.UpdateAsync(id);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        });
    }
}
