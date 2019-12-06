using Frontend.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Frontend.Services
{
    public interface IUserService
    {
        UserModel Authenticate(UserModel user);
    }

    public class UserService : IUserService
    {
        private readonly SecretSettings _secretSettings;
        private readonly IUsersDbContext _userDbContext;

        public UserService(IUsersDbContext dbContext, IOptions<SecretSettings> secretSettings)
        {
            _secretSettings = secretSettings.Value;
            _userDbContext = dbContext;
        }

        public UserModel Authenticate(UserModel user)
        {
            // Gets a user from the database using the username and password.
            UserModel dbUser = _userDbContext.Get(user);

            // return null if user was not found.
            if (dbUser == null)
                return null;

            // Authentication succeful; generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretSettings.SecretString);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, dbUser.Id.ToString())
                }),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            dbUser.Token = tokenHandler.WriteToken(token);

            // Remove password before returning
            dbUser.Password = null;

            return dbUser;
        }

        /// <summary>
        /// Hashes a password using the username as salt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Hashed password string</returns>
        public static string HashPassword(string username, string password)
        {
            if (username.Any() && password.Any())
            {
                string hashed = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: password,
                        salt: Encoding.ASCII.GetBytes(username),
                        prf: KeyDerivationPrf.HMACSHA512,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));

                return hashed;
            }
            else
            {
                return "";
            }
        }
    }
}
