namespace LibraryManagement.Application.Users;

public record LoginResponse(string Username, string AccessToken, int ExpiresIn)
{
}
