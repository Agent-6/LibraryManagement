namespace LibraryManagement.Application.Borrowers;

public record BorrowerResponse(Guid Id, string Name, string Email, string PhoneNumber)
{
}