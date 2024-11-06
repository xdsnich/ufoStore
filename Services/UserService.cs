using Microsoft.EntityFrameworkCore;
using ufoShopBack.Data;
using BCrypt.Net;
using System.Runtime.CompilerServices;
namespace ufoShopBack.Services
   
{
    public class UserService
    {
        private readonly Context _context;

        public UserService(Context context)
        {
            _context = context;
        }

                                                                                        //REGISTRATION 
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUserNameUniqueAsync(string username)
        {
            return !await _context.Users.AnyAsync(x => x.Nickname == username);
        }

        public string HashPassword(string password)
        {
            if (password == null)
            {
                return null;
            }
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
            
                                                                                        //LOG IN
        public bool VerifyPassword(string storedHash, string password)
        {
           
            return  BCrypt.Net.BCrypt.Verify(password, storedHash);
            
        }
        public async Task<bool> ValidateLoginAsync(string password, string email)
        {
            if (password == null && email == null) {
                return false;
            }

            var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDb == null || !VerifyPassword(userFromDb.Password, password)) {

                return false;

            }

            return true;

        } 
    }
}
