using LibraryManagement.Domain.Authors;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

internal class AuthorRepository(LibraryManagementDbContext dbContext) : BaseRepository<Author>(dbContext), IAuthorRepository
{
}
