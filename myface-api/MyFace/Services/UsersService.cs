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
        public (string, string) GetHashedPasswordSalt (string password);
        // public Task<User> Authenticate(string username, string password);
    }

    public class UsersService : IUsersService
    {
        // private readonly IUsersRepo _usersRepo;

        // public UsersService(IUsersRepo usersRepo)
        // {
        //     _usersRepo = usersRepo;
        // }


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

        // public async Task<User> Authenticate(string username, string password)
        // {
        //     UserSearchRequest userSearchRequest = new UserSearchRequest();
        //     userSearchRequest.Search = username;
        //     List<User> users = _usersRepo.Search(userSearchRequest).ToList();

        //     // if (users.Count > 1)
        //     // {
        //     //     return null;
        //     // }
        //     // return null if user not found

        //     if (users.Count > 1 || users == null )
        //         return null;

        //      (var genpassword, var salt) = GetHashedPasswordSalt(password);    

        //     if (users[0].HashedPassword != genpassword)
        //         return null;

        //     // authentication successful so return user details without password
        //     users[0].HashedPassword = null;

        //     return users[0];
        // }

    }
   
}
