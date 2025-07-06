namespace LibraryManagement.Application.Books;

public interface IBookService
{
    Task<List<BookResponse>> GetListAsync();
    Task<BookResponse?> GetAsync(BookRequest request);
    Task<BookResponse?> CreateAsync(BookUpdateRequest request);
    Task<BookResponse?> UpdateAsync(Guid id, BookUpdateRequest request);
    Task DeleteAsync(Guid id);
}
