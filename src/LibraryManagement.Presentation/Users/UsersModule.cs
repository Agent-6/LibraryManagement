using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using LibraryManagement.Application.Users;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Presentation.Users;

public static class UsersModule
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // Login User
        app.MapPost("api/users/login", async (LoginRequest request, IUserService userService) =>
        {
            var response = await userService.LoginAsync(request);
            return Results.Ok(response);
        });

        // Register User
        app.MapPost("/api/users/", async (UserRegistrationRequest request, IUserService userService) =>
        {
            await userService.RegisterAsync(request);
            return Results.Ok();
        });
        
        // Get User
        app.MapGet("/api/users/{id}", async (Guid id, IUserService userService) =>
        {
            var request = new UserRequest(id);

            var response = await userService.GetAsync(request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();

        // Update User
        app.MapPut("/api/users/{id}", async (Guid id, UserUpdateRequest request, IUserService userService) =>
        {
            var response = await userService.UpdateAsync(id, request);
            if (response is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(response);
        }).RequireAuthorization();
    }
}
