using LibraryManagement.Application.Helpers;
using LibraryManagement.Application.Persistance;
using LibraryManagement.Domain.Authors;

namespace LibraryManagement.Application.Authors;

internal class AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork) : IAuthorService
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<AuthorResponse>> GetListAsync()
    {
        var list = await _authorRepository.GetListAsync();

        return [.. list.Select(a => new AuthorResponse(Id: a.Id, Name: a.Name, Bio: a.Bio))];
    }

    public async Task<AuthorResponse?> GetAsync(AuthorRequest request)
    {
        var author = await _authorRepository.GetAsync(request.Id);
        if (author is null) return null; // ??? throw here

        return new(Id: author.Id, Name: author.Name, Bio: author.Bio);
    }

    public async Task<AuthorResponse?> CreateAsync(AuthorUpdateRequest request)
    {
        Check.Empty(request.Name, nameof(request.Name));
        Check.Empty(request.Bio, nameof(request.Bio));

        var author = new Author(request.Name, request.Bio);
        await _authorRepository.AddAsync(author);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: author.Id, Name: author.Name, Bio: author.Bio);
    }

    public async Task<AuthorResponse?> UpdateAsync(Guid id, AuthorUpdateRequest request)
    {
        Check.Empty(request.Name, nameof(request.Name));
        Check.Empty(request.Bio, nameof(request.Bio));

        var author = await _authorRepository.GetAsync(id);
        if (author is null) return null;

        author.Name = request.Name;
        author.Bio = request.Bio;
        await _unitOfWork.SaveChangesAsync();

        return new(Id: author.Id, Name: author.Name, Bio: author.Bio);
    }

    public async Task DeleteAsync(Guid id)
    {
        var author = await _authorRepository.GetAsync(id);
        if (author is null) return;

        await _authorRepository.DeleteAsync(author);
        await _unitOfWork.SaveChangesAsync();
    }
}
