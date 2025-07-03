namespace LibraryManagement.Application.Users;

public record UserUpdateRequest(string Username, string Email, string PhoneNumber)
{
}