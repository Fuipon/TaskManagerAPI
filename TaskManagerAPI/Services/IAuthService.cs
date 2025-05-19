using System.Reflection.Metadata;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IAuthService
    {
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
        string CreateToken(User user);
    }
}
