using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Loans;

public class Loan : AuditedEntity
{
    internal Loan(DateOnly loanDate, Guid borrowerId, Guid bookId)
    {
        LoanDate = loanDate;
        BorrowerId = borrowerId;
        BookId = bookId;
    }

    public DateOnly LoanDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public Guid BorrowerId { get; set; }
    public Guid BookId { get; set; }
}