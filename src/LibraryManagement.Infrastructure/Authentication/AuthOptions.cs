namespace LibraryManagement.Infrastructure.Authentication;

public class AuthOptions
{
    public static string SectionName => "AuthConfig";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int TokenValidityInMins { get; init; }
}
