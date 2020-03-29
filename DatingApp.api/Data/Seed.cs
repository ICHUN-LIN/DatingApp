using System.Collections.Generic;
using DatingApp.api.Models;
using Newtonsoft.Json;
using System.Linq;

namespace DatingApp.api.Data
{
    public class Seed
    {
        public static void SeedUser(DataContext dataContext)
        {

            if(!dataContext.Users.Any()){
                var userdata = System.IO.File.ReadAllText("Data/UserData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userdata);
                foreach(var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password",out passwordHash,out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.Passwordsalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    dataContext.Users.Add(user);
                }
            }

            dataContext.SaveChanges();
            
        }


        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA256())
            {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}