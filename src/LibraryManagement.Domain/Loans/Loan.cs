using LibraryManagement.Domain.Abstracts;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Borrowers;
using System.Text.Json.Serialization;

namespace LibraryManagement.Domain.Loans;

public class Loan : AuditedEntity
{
    public Loan(Borrower borrower, Book book)
    {
        LoanDate = DateOnly.FromDateTime(DateTime.UtcNow);
        BorrowerId = borrower.Id;
        BookId = book.Id;
    }

    [JsonConstructor]
    private Loan(DateOnly loanDate, DateOnly? returnDate, Guid borrowerId, Guid bookId)
    {
        LoanDate = loanDate;
        ReturnDate = returnDate;
        BorrowerId = borrowerId;
        BookId = bookId;
    }

    public DateOnly LoanDate { get; init; }
    public DateOnly? ReturnDate { get; private set; }
    public Guid BorrowerId { get; init; }
    public Guid BookId { get; init; }

    public void ReturnBook()
    {
        if (ReturnDate is not null) return;
        ReturnDate = DateOnly.FromDateTime(DateTime.UtcNow);
    } 
}