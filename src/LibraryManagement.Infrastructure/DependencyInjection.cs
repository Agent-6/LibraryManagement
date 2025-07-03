using LibraryManagement.Domain.Repositories.Users;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) => services
        .ConfigPersistance(configuration);

    private static IServiceCollection ConfigPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryManagementDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        // repositories
        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}
