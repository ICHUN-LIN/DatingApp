using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.api.Models;
using System;
using System.Threading.Tasks;
using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.api.Data
{
    public class DatingRepository : IDatingRepository
    {

        private readonly DataContext _datacontext;

        public DatingRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }
        public void Add<T>(T entity) where T : class
        {
            //doesn't change any thing in database ==> no async
            _datacontext.Add(entity);
        }

        public void Delet<T>(T entity) where T : class
        {
            //doesn't change any thing in database ==> no async
            _datacontext.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userid)
        {
            return await _datacontext.Photos.Where(y => y.Id == userid).FirstOrDefaultAsync(x=>x.IsMain == true);
        }

        public async Task<Photo> GetPhotos(int id)
        {
            return await _datacontext.Photos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _datacontext.Users.Include(p=>p.Photos).FirstOrDefaultAsync(p=>p.id == id);
            //var user = await _datacontext.Users.FirstOrDefaultAsync(p=>p.id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _datacontext.Users.Include(p=>p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _datacontext.SaveChangesAsync()>0;
        }
    }
}