using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage {get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
        int pageNumber, int pageSize) {
            var count = await source.CountAsync();
            //skip : passby a page of number and return one page element
            IQueryable<T> query = source.Skip((pageNumber-1)*pageSize).Take(pageSize);
            //var sql2 = ((System.Data.Entity.Core.Objects.ObjectQuery)query)
            //.ToTraceString();
            Console.WriteLine(query.ToSql<T>());
            var items = await query.ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}