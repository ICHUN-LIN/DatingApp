using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using DatingApp.api.Helpers;
using DatingApp.api.Models;
using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Data {
    public class DatingRepository : IDatingRepository {

        private readonly DataContext _datacontext;

        public DatingRepository (DataContext dataContext) {
            _datacontext = dataContext;
        }
        public void Add<T> (T entity) where T : class {
            //doesn't change any thing in database ==> no async
            _datacontext.Add (entity);
        }

        public void Delet<T> (T entity) where T : class {
            //doesn't change any thing in database ==> no async
            _datacontext.Remove (entity);
        }

        public async Task<Photo> GetMainPhotoForUser (int userid) {
            return await _datacontext.Photos.Where (y => y.Id == userid).FirstOrDefaultAsync (x => x.IsMain == true);
        }

        public async Task<Photo> GetPhotos (int id) {
            return await _datacontext.Photos.FirstOrDefaultAsync (x => x.Id == id);
        }

        public async Task<User> GetUser (int id) {
            var user = await _datacontext.Users.Include (p => p.Photos).FirstOrDefaultAsync (p => p.id == id);
            //var user = await _datacontext.Users.FirstOrDefaultAsync(p=>p.id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers (UserParams userParams) {
            //IQuerable: setence you havn't really Query
            //need use AsQueryable to let following "where" function can work
            var users = _datacontext.Users.Include (p => p.Photos).AsQueryable ();
            users = users.Where (a => a.id != userParams.UserID);
            users = users.Where (x => x.Gender == userParams.Gender);
            if (userParams.MinAge != 18 || userParams.MaxAge != 99) {
                DateTime minB = DateTime.Now.AddYears (-userParams.MaxAge - 1);
                DateTime maxB = DateTime.Now.AddYears (-userParams.MinAge);
                users = users.Where (x => x.DateOfBirth >= minB && x.DateOfBirth < maxB);
            }

            if (!string.IsNullOrEmpty (userParams.orderBy)) {
                switch (userParams.orderBy) {
                    case "created":
                        users = users.OrderByDescending (u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending (x => x.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync (users, userParams.Pagenumbers, userParams.PageSize);
        }

        public async Task<bool> SaveAll () {
            return await _datacontext.SaveChangesAsync () > 0;
        }
    }
}