using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace LibraryManagement.Infrastructure.Authentication.Helpers;

public static class PasswordHashHelper
{
    private static readonly int _iterationCount = 100_000;
    private static readonly KeyDerivationPrf _prf = KeyDerivationPrf.HMACSHA512; 
    private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

    public static string HashPassword(string password)
    {
        int saltSize = 128 / 8; // 128 bits
        var salt = new byte[saltSize];
        _randomNumberGenerator.GetBytes(salt);

        int keyLength = 256 / 8; // 256 bits
        var subkey = KeyDerivation.Pbkdf2(password, salt, _prf, _iterationCount, keyLength);

        var outputBytes = new byte[12 + salt.Length + subkey.Length];
        
        WriteNetworkByteOrder(outputBytes, 0, (uint)_prf);
        WriteNetworkByteOrder(outputBytes, 4, (uint)_iterationCount);
        WriteNetworkByteOrder(outputBytes, 8, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 12, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 12 + saltSize, subkey.Length);
        
        return Convert.ToBase64String(outputBytes);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword));
        
        if (string.IsNullOrWhiteSpace(providedPassword))
            throw new ArgumentNullException(nameof(providedPassword));

        byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        try
        {
            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 0);
            var iterationCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 4);
            int saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 8);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            byte[] salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 12, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            int subkeyLength = decodedHashedPassword.Length - 12 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            byte[] expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 12 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterationCount, subkeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
        }
        catch
        {
            return false;
        }
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
            | ((uint)(buffer[offset + 1]) << 16)
            | ((uint)(buffer[offset + 2]) << 8)
            | ((uint)(buffer[offset + 3]));
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
}
