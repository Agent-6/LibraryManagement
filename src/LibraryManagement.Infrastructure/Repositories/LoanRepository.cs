using LibraryManagement.Domain.Loans;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

internal class LoanRepository(LibraryManagementDbContext dbContext) : BaseRepository<Loan>(dbContext), ILoanRepository
{
    public Task<Loan?> GetByBorrowerAndBookAsync(Guid borrowerId, Guid bookId)
    {
        return DbSet.Where(l => l.BorrowerId == borrowerId && l.BookId == bookId).FirstOrDefaultAsync();
    }
}
