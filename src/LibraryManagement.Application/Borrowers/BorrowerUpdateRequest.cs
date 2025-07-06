namespace LibraryManagement.Application.Borrowers;

public record BorrowerUpdateRequest(string Name, string Email, string PhoneNumber)
{
}