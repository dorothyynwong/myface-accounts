using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFace.Services 
{
    public interface IUsersService
    {
        public (string, string) GetHashedPasswordSalt (string password);
    }

    public class UsersService : IUsersService
    {
        private MyFaceDbContext _context;
        public (string, string) GetHashedPasswordSalt (string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return (hashed, Convert.ToBase64String(salt));
        }
    }
}
