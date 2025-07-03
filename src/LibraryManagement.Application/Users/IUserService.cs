namespace LibraryManagement.Application.Users;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(UserRegistrationRequest request);
    Task<UserResponse?> GetAsync(UserRequest request);
    Task<UserResponse?> UpdateAsync(Guid id, UserUpdateRequest request);
}
