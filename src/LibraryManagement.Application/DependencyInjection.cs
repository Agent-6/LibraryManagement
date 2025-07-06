using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthorService, AuthorService>();
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IUserService, UserService>();
        return services;
    }
}
