using LibraryManagement.Domain.Borrowers;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

internal class BorrowerRepository(LibraryManagementDbContext dbContext) : BaseRepository<Borrower>(dbContext), IBorrowerRepository
{
}
