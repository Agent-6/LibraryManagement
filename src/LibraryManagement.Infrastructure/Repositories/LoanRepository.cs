using LibraryManagement.Domain.Loans;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

internal class LoanRepository(LibraryManagementDbContext dbContext) : BaseRepository<Loan>(dbContext), ILoanRepository
{
}
