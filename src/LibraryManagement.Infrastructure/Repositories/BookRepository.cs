using LibraryManagement.Domain.Books;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

internal class BookRepository(LibraryManagementDbContext dbContext) : BaseRepository<Book>(dbContext), IBookRepository
{
}
