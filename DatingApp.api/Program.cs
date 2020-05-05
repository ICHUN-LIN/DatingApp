using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DatingApp.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DatingApp.api.Models;

namespace DatingApp.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //creat seed data here
            var host = CreateHostBuilder(args).Build();
            using(var scope=host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var datacontext = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    //do migration aoutmatically: datacontext.Database.Migrate();
                    //Seed.SeedUser(datacontext);
                    Seed.SeedUser(userManager, roleManager);

                }
                catch(Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e,"error happen durning SeedUsers");
                    
                }

            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
