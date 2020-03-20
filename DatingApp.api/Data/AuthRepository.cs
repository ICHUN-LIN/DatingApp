using System;
using System.Threading.Tasks;
using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        public AuthRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;

        }
        async Task<User> IAuthRepository.Login(string username, string password)
        {
            var user = await this._dataContext.Users.FirstOrDefaultAsync(x=>x.UserName == username);
            
            if(user == null)
            {
                return null;
            }
            
            if(!VerifyPasswordHash(password, user.PasswordHash, user.Passwordsalt))
                return null;


            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordsalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA256(passwordsalt))
            {
               var _passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
              for(int i=0; i<_passwordHash.Length; i++)
              {
                  if(_passwordHash[i]!=passwordHash[i]) return false;
              }

              return true;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA256())
            {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        async Task<User> IAuthRepository.Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.Passwordsalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        async Task<bool> IAuthRepository.UserExist(string username)
        {
            if(await _dataContext.Users.AnyAsync(x=>x.UserName == username))
                return true;
            else
            {
                return false;
            }
        }


    }
}