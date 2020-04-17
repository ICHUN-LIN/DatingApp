using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        public static void AddPagination(this HttpResponse response,int currentPages, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPages,itemsPerPage, totalItems, totalPages);
            //Use camel Case Formatting
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
        

        public static int CaculateYear(this DateTime birthday)
        {
            var age = DateTime.Now.Year -birthday.Year;
            if(birthday.AddYears(age)>DateTime.Now)
            {
                age --;
            }
            return age;
        }
    }
}