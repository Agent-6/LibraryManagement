namespace LibraryManagement.Application.Loans;

public interface ILoanService
{
    Task<List<LoanResponse>> GetListAsync();
    Task<LoanResponse?> CreateAsync(LoanCreateRequest request);
    Task<LoanResponse?> UpdateAsync(Guid id);
}
