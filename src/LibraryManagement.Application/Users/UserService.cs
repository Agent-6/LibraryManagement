using LibraryManagement.Application.Services.Authentication;
using LibraryManagement.Domain.Repositories.Users;

namespace LibraryManagement.Application.Users;

internal class UserService(IAuthenticationProvider authenticationProvider, IUserRepository userRepository) : IUserService
{
    private readonly IAuthenticationProvider _authenticationProvider = authenticationProvider;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<LoginResponse> LoginAsync(LoginRequest request) =>
        await _authenticationProvider.AuthenticateAsync(request);

    public async Task RegisterAsync(UserRegistrationRequest request) =>
        await _authenticationProvider.RegisterAsync(request);

    public async Task<UserResponse?> GetAsync(UserRequest request)
    {
        var user = await _userRepository.GetAsync(request.Id);
        if (user is null) return null; // ??? throw here

        return new(Id: user.Id, Username: user.Username, Email: user.Email, PhoneNumber: user.PhoneNumber);
    }

    public async Task<UserResponse?> UpdateAsync(Guid id, UserUpdateRequest request)
    {
        CheckEmpty(request.Username, nameof(request.Username));
        CheckEmpty(request.Email, nameof(request.Email));
        CheckEmpty(request.PhoneNumber, nameof(request.PhoneNumber));

        var user = await _userRepository.GetAsync(id);
        if (user is null) return null; // ??? throw here

        user.Username = request.Username;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;

        return new(Id: user.Id, Username: user.Username, Email: user.Email, PhoneNumber: user.PhoneNumber);
    }

    private static void CheckEmpty(string paramValue, string paramName)
    {
        if (string.IsNullOrEmpty(paramValue))
            throw new ArgumentNullException(paramName);
    }
}
