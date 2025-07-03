namespace LibraryManagement.Application.Users;

public record UserRegistrationRequest(string Username, string Email, string PhoneNumber, string Password, string ConfirmPassword)
{
}