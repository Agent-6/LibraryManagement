namespace LibraryManagement.Application.Loans;

public record LoanCreateRequest(Guid BorrowerId, Guid BookId)
{
}