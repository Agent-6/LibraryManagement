using LibraryManagement.Domain.Users;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

internal class UserRepository(LibraryManagementDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
}
