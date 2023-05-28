using System.Security.Cryptography;

namespace Ticketing.Auth.Application.Services;

public static class PasswordService
{
    private const int SaltSize = 8;
    private const int HashSize = 64;
    private const int Iterations = 10000;

    public static async Task<string> ToHash(string password)
    {
        var saltBytes = GenerateSalt();
        var hashBytes = await GenerateHash(password, saltBytes);

        var salt = Convert.ToHexString(saltBytes);
        var hash = Convert.ToHexString(hashBytes);

        return $"{hash}.{salt}";
    }

    public static async Task<bool> Compare(string storedPassword, string suppliedPassword)
    {
        var passwordParts = storedPassword.Split('.');
        if (passwordParts.Length != 2)
            return false;

        var hashBytes = Convert.FromHexString(passwordParts[0]);
        var saltBytes = Convert.FromHexString(passwordParts[1]);

        var suppliedHashBytes = await GenerateHash(suppliedPassword, saltBytes);

        return ByteArraysEqual(hashBytes, suppliedHashBytes);
    }

    private static byte[] GenerateSalt()
    {
        var saltBytes = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return saltBytes;
    }

    private static async Task<byte[]> GenerateHash(string password, byte[] salt)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hashBytes = await Task.Run(() => deriveBytes.GetBytes(HashSize));
        return hashBytes;
    }

    private static bool ByteArraysEqual(IReadOnlyCollection<byte> a, IReadOnlyList<byte> b)
    {
        if (a.Count != b.Count)
            return false;

        return !a.Where((t, i) => t != b[i]).Any();
    }
}