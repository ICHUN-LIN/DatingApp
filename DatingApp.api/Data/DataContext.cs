using Microsoft.EntityFrameworkCore;
using DatingApp.api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.api.Data
{
    //<User, Role, int> ==> wich class you want to use as User, Role, Or Id
   // Change to inherit from IdentityDbContext ==> add Role Management ==> create many new Table for user login and handling
   // public class DataContext : DbContext
    public class DataContext: IdentityDbContext<User, Role, int, 
    IdentityUserClaim<int>, UserRole, 
    IdentityUserLogin<int>, IdentityRoleClaim<int>, 
    IdentityUserToken<int>>
    {
        
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        public DbSet<Value> Values {get; set;}

       // public DbSet<User> Users {get; set;}

        public DbSet<Photo> Photos {get; set;}

        public DbSet<Like> Likes {get; set;}

        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);
            builder.Entity<UserRole>( userrole =>{
                userrole.HasKey(k=> new {k.UserId, k.RoleId});
                userrole.HasOne(r=> r.User).WithMany(u => u.UserRoles ).HasForeignKey( r=> r.UserId).IsRequired();
                userrole.HasOne(r => r.Role).WithMany(u => u.UserRoles).HasForeignKey( r=>r.RoleId).IsRequired();
            });
            //set primary key
            builder.Entity<Like>().HasKey(k => new {k.LikerId, k.LikeeId});
            builder.Entity<Like>().HasOne(x=>x.Likee).WithMany(y=>y.Likers).
                HasForeignKey(z=>z.LikeeId).OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Like>().HasOne(x=>x.Liker).WithMany(y=>y.Likees).
                HasForeignKey(z=>z.LikerId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
