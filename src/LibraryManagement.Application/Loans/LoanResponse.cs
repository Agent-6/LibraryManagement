namespace LibraryManagement.Application.Loans;

public record LoanResponse(Guid Id, DateOnly LoanDate, DateOnly? ReturnDate, Guid BorrowerId, Guid BookId)
{
}