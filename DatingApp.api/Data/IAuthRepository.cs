using System.Threading.Tasks;
using DatingApp.api.Models;


namespace DatingApp.api.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);

         Task<bool> UserExist(string username);

         
         Task<User>  Login (string username, string password);

    }
}