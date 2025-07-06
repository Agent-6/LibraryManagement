namespace LibraryManagement.Application.Borrowers;

public interface IBorrowerService
{
    Task<List<BorrowerResponse>> GetListAsync();
    Task<BorrowerResponse?> GetAsync(BorrowerRequest request);
    Task<BorrowerResponse?> CreateAsync(BorrowerUpdateRequest request);
    Task<BorrowerResponse?> UpdateAsync(Guid id, BorrowerUpdateRequest request);
    Task DeleteAsync(Guid id);
}
