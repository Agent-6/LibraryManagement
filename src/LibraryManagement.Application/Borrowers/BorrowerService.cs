using LibraryManagement.Application.Helpers;
using LibraryManagement.Application.Persistance;
using LibraryManagement.Domain.Borrowers;

namespace LibraryManagement.Application.Borrowers;

internal class BorrowerService(IBorrowerRepository borrowerRepository, IUnitOfWork unitOfWork) : IBorrowerService
{
    private readonly IBorrowerRepository _borrowerRepository = borrowerRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<BorrowerResponse>> GetListAsync()
    {
        var list = await _borrowerRepository.GetListAsync();

        return [.. list.Select(b => new BorrowerResponse(Id: b.Id, Name: b.Name, Email: b.Email, PhoneNumber: b.PhoneNumber))];
    }

    public async Task<BorrowerResponse?> GetAsync(BorrowerRequest request)
    {
        var borrower = await _borrowerRepository.GetAsync(request.Id);
        if (borrower is null) return null; // ??? throw here

        return new(Id: borrower.Id, Name: borrower.Name, Email: borrower.Email, PhoneNumber: borrower.PhoneNumber);
    }

    public async Task<BorrowerResponse?> CreateAsync(BorrowerUpdateRequest request)
    {
        Check.Empty(request.Name, nameof(request.Name));
        Check.Empty(request.Email, nameof(request.Email));
        Check.Empty(request.PhoneNumber, nameof(request.PhoneNumber));

        var borrower = new Borrower(name: request.Name,
                                    email: request.Email,
                                    phoneNumber: request.PhoneNumber);

        await _borrowerRepository.AddAsync(borrower);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: borrower.Id, Name: borrower.Name, Email: borrower.Email, PhoneNumber: borrower.PhoneNumber);
    }

    public async Task<BorrowerResponse?> UpdateAsync(Guid id, BorrowerUpdateRequest request)
    {
        Check.Empty(request.Name, nameof(request.Name));
        Check.Empty(request.Email, nameof(request.Email));
        Check.Empty(request.PhoneNumber, nameof(request.PhoneNumber));

        var borrower = await _borrowerRepository.GetAsync(id);
        if (borrower is null) return null;

        borrower.Name = request.Name;
        borrower.Email = request.Email;
        borrower.PhoneNumber = request.PhoneNumber;

        await _borrowerRepository.UpdateAsync(borrower);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: borrower.Id, Name: borrower.Name, Email: borrower.Email, PhoneNumber: borrower.PhoneNumber);
    }

    public async Task DeleteAsync(Guid id)
    {
        var borrower = await _borrowerRepository.GetAsync(id);
        if (borrower is null) return;

        await _borrowerRepository.DeleteAsync(borrower);
        await _unitOfWork.SaveChangesAsync();
    }
}
