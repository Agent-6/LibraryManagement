using LibraryManagement.Application.Services.Authentication;
using LibraryManagement.Domain.Repositories.Users;
using LibraryManagement.Domain.Users;
using LibraryManagement.Infrastructure.Authentication;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) => services
        .ConfigPersistance(configuration);

    private static IServiceCollection ConfigAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfig = configuration.GetSection(AuthOptions.SectionName);
        var authOptions = authConfig.Get<AuthOptions>();
        services.Configure<AuthOptions>(authConfig);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var keyBytes = Encoding.UTF8.GetBytes(authOptions?.Key!);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidIssuer = authOptions?.Issuer,
                ValidAudience = authOptions?.Audience,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

        services.AddAuthorization();

        // services
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddTransient<IAuthenticationProvider, AuthenticationService>();

        return services;
    }

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
