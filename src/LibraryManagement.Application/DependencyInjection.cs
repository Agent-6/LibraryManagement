using LibraryManagement.Application.Authors;
using LibraryManagement.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthorService, AuthorService>();
        services.AddTransient<IUserService, UserService>();
        return services;
    }
}
