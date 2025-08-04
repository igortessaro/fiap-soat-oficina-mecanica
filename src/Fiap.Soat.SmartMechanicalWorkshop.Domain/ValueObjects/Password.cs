using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public record Password
{
    public Password() { }

    private Password(string password)
    {
        if (!IsValid(password))
        {
            throw new DomainException("Password must be at least 8 characters long, contain at least one letter, one digit, and one special character.");
        }
        Value = HashPassword(password);
    }

    public string Value { get; private set; } = string.Empty;

    public static bool IsValid(string password)
    {
        if (password.Length < 8) return false;
        bool hasLetter = Regex.IsMatch(password, "[a-zA-Z]", RegexOptions.NonBacktracking);
        bool hasDigit = Regex.IsMatch(password, @"\d", RegexOptions.NonBacktracking);
        bool hasSpecial = Regex.IsMatch(password, @"[\W_]", RegexOptions.NonBacktracking);
        return hasLetter && hasDigit && hasSpecial;
    }

    public bool VerifyPassword(string password)
    {
        string[] parts = Value.Split('.');
        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] hash = Convert.FromBase64String(parts[1]);
        byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static implicit operator Password(string value) => new Password(value);
    public static implicit operator string(Password password) => password.Value;
}
