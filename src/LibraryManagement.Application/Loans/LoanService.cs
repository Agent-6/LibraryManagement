using LibraryManagement.Application.Helpers;
using LibraryManagement.Application.Persistance;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Borrowers;
using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Application.Loans;

internal class LoanService(ILoanRepository loanRepository,
                           IBorrowerRepository borrowerRepository,
                           IBookRepository bookRepository,
                           IUnitOfWork unitOfWork) : ILoanService
{
    private readonly ILoanRepository _loanRepository = loanRepository;
    private readonly IBorrowerRepository _borrowerRepository = borrowerRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<LoanResponse>> GetListAsync()
    {
        var list = await _loanRepository.GetListAsync();

        return [.. list.Select(l => new LoanResponse(Id: l.Id, LoanDate: l.LoanDate, ReturnDate: l.ReturnDate, BorrowerId: l.BorrowerId, BookId: l.BookId))];
    }

    public async Task<LoanResponse?> CreateAsync(LoanCreateRequest request)
    {
        Check.Empty(request.BorrowerId, nameof(request.BorrowerId));
        Check.Empty(request.BookId, nameof(request.BookId));

        var borrower = await _borrowerRepository.GetAsync(request.BorrowerId);
        if (borrower is null) return null;

        var book = await _bookRepository.GetAsync(request.BookId);
        if (book is null) return null;

        // check if book is already loaned to this borrower
        var loan = await _loanRepository.GetByBorrowerAndBookAsync(borrower.Id, book.Id);
        if (loan is not null)
        {
            throw new Exception("Book already loaned");
        }

        loan = new Loan(borrower, book);
        await _loanRepository.AddAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: loan.Id, LoanDate: loan.LoanDate, ReturnDate: loan.ReturnDate, BorrowerId: loan.BorrowerId, BookId: loan.BookId);
    }

    public async Task<LoanResponse?> UpdateAsync(Guid id)
    {
        var loan = await _loanRepository.GetAsync(id);
        if (loan is null) return null;

        loan.ReturnBook();
        await _loanRepository.UpdateAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: loan.Id, LoanDate: loan.LoanDate, ReturnDate: loan.ReturnDate, BorrowerId: loan.BorrowerId, BookId: loan.BookId);
    }
}
