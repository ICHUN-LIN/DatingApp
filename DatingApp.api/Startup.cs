using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.api.Data;
using DatingApp.api.Helpers;
using DatingApp.api.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DatingApp.api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<DataContext> (x => x.UseLoggerFactory (DataContext.MyLoggerFactory).UseSqlite (Configuration.GetConnectionString ("Default")));
            //AddNewtonsoftJson: use new json format to return, It's slight different than it recently 
            services.AddControllers ().AddNewtonsoftJson (
                options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }
            );
            //create limitation
            IdentityBuilder builder = services.AddIdentityCore<User> (opt => {
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
            });
            //all configuration in Identity
            builder = new IdentityBuilder (builder.UserType, typeof (Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext> ();
            builder.AddRoleValidator<RoleValidator<Role>> ();
            builder.AddRoleManager<RoleManager<Role>> ();
            builder.AddSignInManager<SignInManager<User>> ();
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer (Options =>
                Options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (Configuration.GetSection ("AppSettings:token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                }
            );

            //add role policy to authorization
            services.AddAuthorization(Options=>
            {
                Options.AddPolicy("RequireAdminRole",policy=>policy.RequireRole("Admin"));
                Options.AddPolicy("ModeratePhotoRole",policy=>policy.RequireRole("Admin","Moderator"));
                Options.AddPolicy("VipOnly",policy=>policy.RequireRole("VIP"));

            });

            //add authorFilter : by default => this is going to add every Authoriztion in every method
            services.AddControllers (Options => {
                var police = new AuthorizationPolicyBuilder ().
                RequireAuthenticatedUser ().Build ();
                Options.Filters.Add (new AuthorizeFilter (police));

            }).AddNewtonsoftJson (opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors ();
            //auto map content from configuration("X") to <Y>
            services.Configure<CloudinarySettings> (Configuration.GetSection ("CloudinarySettings"));
            //what is assembly means? which asembly you are look in for profiles.
            services.AddAutoMapper (typeof (DatingApp.api.Helpers.AutoMapperProfiles).Assembly);
            services.AddScoped<IAuthRepository, AuthRepository> ();
            services.AddScoped<IDatingRepository, DatingRepository> ();
            //add instance in one request
            services.AddScoped<LogUserActivity> ();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) { //running in Development mode
                //return friendly development page while exception happen
                app.UseDeveloperExceptionPage ();
            } else {
                //builder as options
                app.UseExceptionHandler (builder => {
                    builder.Run (async context => {
                        //gloable set StatusCode
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        //gloable get exception message from excepption
                        var error = context.Features.Get<IExceptionHandlerFeature> ();
                        if (error != null) {
                            context.Response.AddApplicationError (error.Error.Message);
                            await context.Response.WriteAsync (error.Error.Message);
                        }
                    });
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting ();

            //app.UseAuthorization();
            //alow any domain to call and any head
            app.UseCors (x => x.AllowAnyOrigin ().AllowAnyMethod ().AllowAnyHeader ());
            app.UseAuthentication ();
            app.UseAuthorization ();
            //app.UseMvc();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });

        }
    }
}