using LibraryManagement.Application.Users;

namespace LibraryManagement.Application.Services.Authentication;

public interface IAuthenticationProvider
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest request);

    Task RegisterAsync(UserRegistrationRequest request);
}
