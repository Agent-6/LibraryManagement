using LibraryManagement.Application.Helpers;
using LibraryManagement.Application.Persistance;
using LibraryManagement.Domain.Authors;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Books;

internal class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IUnitOfWork unitOfWork) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<BookResponse>> GetListAsync()
    {
        var list = await _bookRepository.GetListAsync();

        return [.. list.Select(b => new BookResponse(Id: b.Id, Title: b.Title, ISBN: b.ISBN, PublishDate: b.PublishDate, AuthorId: b.AuthorId))];
    }

    public async Task<BookResponse?> GetAsync(BookRequest request)
    {
        var book = await _bookRepository.GetAsync(request.Id);
        if (book is null) return null; // ??? throw here

        return new(Id: book.Id, Title: book.Title, ISBN: book.ISBN, PublishDate: book.PublishDate, AuthorId: book.AuthorId);
    }

    public async Task<BookResponse?> CreateAsync(BookUpdateRequest request)
    {
        Check.Empty(request.Title, nameof(request.Title));
        Check.Empty(request.ISBN, nameof(request.ISBN));
        Check.Empty(request.AuthorId, nameof(request.AuthorId));

        var author = await _authorRepository.GetAsync(request.AuthorId);
        if (author is null) return null;

        var book = new Book(title: request.Title,
                            iSBN: request.ISBN,
                            publishDate: request.PublishDate,
                            author: author);

        await _bookRepository.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: book.Id, Title: book.Title, ISBN: book.ISBN, PublishDate: book.PublishDate, AuthorId: book.AuthorId);
    }

    public async Task<BookResponse?> UpdateAsync(Guid id, BookUpdateRequest request)
    {
        Check.Empty(request.Title, nameof(request.Title));
        Check.Empty(request.ISBN, nameof(request.ISBN));
        Check.Empty(request.AuthorId, nameof(request.AuthorId));

        var author = await _authorRepository.GetAsync(request.AuthorId);
        if (author is null) return null;

        var book = await _bookRepository.GetAsync(id);
        if (book is null) return null;

        book.Title = request.Title;
        book.ISBN = request.ISBN;
        book.PublishDate = request.PublishDate;
        book.SetAuthor(author);

        await _bookRepository.UpdateAsync(book);
        await _unitOfWork.SaveChangesAsync();

        return new(Id: book.Id, Title: book.Title, ISBN: book.ISBN, PublishDate: book.PublishDate, AuthorId: book.AuthorId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetAsync(id);
        if (book is null) return;

        await _bookRepository.DeleteAsync(book);
        await _unitOfWork.SaveChangesAsync();
    }
}
