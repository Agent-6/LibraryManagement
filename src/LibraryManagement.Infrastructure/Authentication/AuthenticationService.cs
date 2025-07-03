using LibraryManagement.Application.Services.Authentication;
using LibraryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JsonWebTokens = Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using LibraryManagement.Application.Users;
using LibraryManagement.Domain.Users;

namespace LibraryManagement.Infrastructure.Authentication;

internal class AuthenticationService(LibraryManagementDbContext dbContext, IPasswordHasher<User> passwordHasher, IOptions<AuthOptions> options) : IAuthenticationProvider
{
    private readonly LibraryManagementDbContext _dbContext = dbContext;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly AuthOptions _options = options.Value;

    public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
    {
        CheckEmpty(request.Username, nameof(request.Username));
        CheckEmpty(request.Password, nameof(request.Password));

        User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username);
        if (user is null)
        {
            throw new Exception($"User with username [{request.Username}] was not found.");
        }
        
        if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) is not PasswordVerificationResult.Success)
        {
            throw new Exception($"Incorrect User Credintials.");
        }

        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(_options.TokenValidityInMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JsonWebTokens.JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber),
            ]),
            Expires = tokenExpiryTimeStamp,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!)),
                SecurityAlgorithms.HmacSha512Signature
            ),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);
        var expiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds;

        return new LoginResponse(Username: user.Username, AccessToken: accessToken, ExpiresIn: expiresIn);
    }

    public async Task RegisterAsync(UserRegistrationRequest request)
    {
        // validation
        CheckEmpty(request.Username, nameof(request.Username));
        CheckEmpty(request.Email, nameof(request.Email));
        CheckEmpty(request.PhoneNumber, nameof(request.PhoneNumber));
        CheckEmpty(request.Password, nameof(request.Password));
        CheckEmpty(request.ConfirmPassword, nameof(request.ConfirmPassword));

        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException($"Password [{request.Password}] and Confirmed Password [{request.ConfirmPassword}] do not match");
        }

        var hashedPassword = _passwordHasher.HashPassword(null!, request.Password);
        var user = new User(Guid.NewGuid(), request.Username, hashedPassword, request.Email, request.PhoneNumber);

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    private static void CheckEmpty(string paramValue, string paramName)
    {
        if (string.IsNullOrEmpty(paramValue))
            throw new ArgumentNullException(paramName);
    }
}
