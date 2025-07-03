using LibraryManagement.Domain.Users;

namespace LibraryManagement.Domain.Repositories.Users;

public interface IUserRepository
{
    /// <summary>
    /// Gets the User with the given Id. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The User if found; else <see langword="null"/>.</returns>
    Task<User?> GetAsync(Guid id);
}
