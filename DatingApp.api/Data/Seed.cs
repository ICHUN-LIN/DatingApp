using System.Collections.Generic;
using System.Linq;
using DatingApp.api.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.api.Data {
    public class Seed {
        public static void SeedUser (UserManager<User> userManager, RoleManager<Role> roleManager) {

            if (!userManager.Users.Any ()) {
                var userdata = System.IO.File.ReadAllText ("Data/UserData.json");
                var users = JsonConvert.DeserializeObject<List<User>> (userdata);

                //create user type
                List<Role> rolelist = new List<Role> () {
                    new Role { Name = "Member" },
                    new Role { Name = "Admin" },
                    new Role { Name = "Moderator" },
                    new Role { Name = "VIP" }
                };

                foreach (var role in rolelist) {
                    //create role
                    roleManager.CreateAsync (role).Wait ();
                }

                foreach (var user in users) {
                    //byte[] passwordHash, passwordSalt;
                    //CreatePasswordHash("password",out passwordHash,out passwordSalt);
                    //user.PasswordHash = passwordHash;
                    //user.Passwordsalt = passwordSalt;
                    //user.UserName = user.UserName.ToLower();
                    //dataContext.Users.Add(user);
                    userManager.CreateAsync (user, "password").Wait ();
                    userManager.AddToRoleAsync (user, "Member").Wait ();
                }

                var adminUser = new User {
                    UserName = "Admin"
                };
                var result = userManager.CreateAsync (adminUser, "password").Result;
                if (result.Succeeded) {
                    var admin = userManager.FindByNameAsync (adminUser.UserName).Result;
                    userManager.AddToRolesAsync (admin, new [] { "Admin", "Moderator" });
                }
            }

            //dataContext.SaveChanges();

        }

        private static void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA256 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }
    }
}