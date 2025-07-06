namespace LibraryManagement.Application.Authors;

public interface IAuthorService
{
    Task<List<AuthorResponse>> GetListAsync();
    Task<AuthorResponse?> GetAsync(AuthorRequest request);
    Task<AuthorResponse?> CreateAsync(AuthorUpdateRequest request);
    Task<AuthorResponse?> UpdateAsync(Guid id, AuthorUpdateRequest request);
    Task DeleteAsync(Guid id);
}
