using Microsoft.EntityFrameworkCore;
using DatingApp.api.Models;
using Microsoft.Extensions.Logging;

namespace DatingApp.api.Data
{
    public class DataContext : DbContext
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

        public DbSet<User> Users {get; set;}

        public DbSet<Photo> Photos {get; set;}
    }
}
