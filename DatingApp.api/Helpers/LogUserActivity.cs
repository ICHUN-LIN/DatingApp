using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using DatingApp.api.Data;
using System;

namespace DatingApp.api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        //context: doing during action, next is doing after action
        public async Task OnActionExecutionAsync(ActionExecutingContext context, 
        ActionExecutionDelegate next)
        {
            //throw new System.NotImplementedException();
            var resultContext = await next();
            var userid = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repositry = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            var user = await repositry.GetUser(userid);
            user.LastActive = DateTime.Now;
            await repositry.SaveAll();

        }
    }
}