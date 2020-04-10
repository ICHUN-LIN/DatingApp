using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DatingApp.api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.api.Helpers;
using Newtonsoft.Json;
using AutoMapper;


namespace DatingApp.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
            //AddNewtonsoftJson: use new json format to return, It's slight different than it recently 
            services.AddControllers().AddNewtonsoftJson(
                options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
            );
            services.AddCors();
            //auto map content from configuration("X") to <Y>
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            //what is assembly means? which asembly you are look in for profiles.
            services.AddAutoMapper(typeof(DatingApp.api.Helpers.AutoMapperProfiles).Assembly);
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository,DatingRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options=>
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {   //running in Development mode
                //return friendly development page while exception happen
                app.UseDeveloperExceptionPage();
            }else
            {
                //builder as options
                app.UseExceptionHandler(builder=>{
                    builder.Run(async context => {
                        //gloable set StatusCode
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        //gloable get exception message from excepption
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if(error!=null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);                        }
                    });
                });
            }

            //app.UseHttpsRedirection();
      
            app.UseRouting();

            //app.UseAuthorization();
            //alow any domain to call and any head
            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMvc();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
