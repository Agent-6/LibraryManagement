using LibraryManagement.Domain.Abstracts;

namespace LibraryManagement.Domain.Loans;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetByBorrowerAndBookAsync(Guid borrowerId, Guid bookId);
}
