using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DatingApp.api.Models;

namespace DatingApp.api.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T : class;

         void Delet<T>(T entity) where T : class;

         Task<bool> SaveAll();

         Task<IEnumerable<User>> GetUsers();

         Task<User> GetUser(int id);
    }
}