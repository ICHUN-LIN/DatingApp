using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.api.Helpers;

namespace DatingApp.api.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T : class;

         void Delet<T>(T entity) where T : class;

         Task<bool> SaveAll();

         Task<PagedList<User>> GetUsers(UserParams userParams);

         Task<User> GetUser(int id);

         Task<Photo> GetPhotos(int id);
         Task<Photo> GetMainPhotoForUser(int userid);
    }
}