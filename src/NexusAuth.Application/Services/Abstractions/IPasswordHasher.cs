using NexusAuth.Application.Models;

namespace NexusAuth.Application.Services.Abstractions
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHashString);
        CryptoParameter GetParametersFromHash(string storedHashString);
    }
}
