using Microsoft.AspNetCore.Http;

namespace DatingApp.api.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string errmsg)
        {
            response.Headers.Add("App-error",errmsg);
            //left first one can pass
            response.Headers.Add("Access-Control-Expose-Headers","app-error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }
        
    }
}