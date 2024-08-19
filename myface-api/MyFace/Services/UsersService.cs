using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MyFace.Models.Database;
using MyFace.Models.Request;
using MyFace.Repositories;

namespace MyFace.Services 
{
    public interface IUsersService
    {
        public (string, string) GetHashedPasswordSalt (string password, string saltString);
    }

    public class UsersService : IUsersService
    {
        
        public (string, string) GetHashedPasswordSalt (string password, string saltString = "")
        {
            byte[] salt = new byte[128 / 8];
            if (saltString == "")
            {
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
            }
            else
            {
                salt = Convert.FromBase64String(saltString);
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
