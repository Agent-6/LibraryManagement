using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Users;

public class User(Guid id, string username, string password, string email, string phoneNumber) : Entity(id)
{
    public string Username { get; set; } = username;
    public string Password { get; private init; } = password;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;
}