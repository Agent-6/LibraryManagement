namespace LibraryManagement.Application.Users;

public record UserResponse(Guid Id, string Username, string Email, string PhoneNumber)
{
}