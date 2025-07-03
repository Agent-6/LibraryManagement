using LibraryManagement.Domain.Repositories.Users;
using LibraryManagement.Domain.Users;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories.Users;

internal class UserRepository(LibraryManagementDbContext dbContext) : IUserRepository
{
    private readonly LibraryManagementDbContext _dbContext = dbContext;

    public async Task<User?> GetAsync(Guid id) => await _dbContext.Users.FindAsync(id);
}
