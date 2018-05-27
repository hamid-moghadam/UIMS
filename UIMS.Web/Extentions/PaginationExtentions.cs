using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;

namespace UIMS.Web.Extentions
{
    public static class PagingExtensions
    {
        public async static Task<PaginationViewModel<TSource>> ToPageAsync<TSource>(this IQueryable<TSource> source, int pageSize, int page)
        {
            var data = source.Skip((page - 1) * pageSize).Take(pageSize);
            var sourceCount = await source.CountAsync();
            return new PaginationViewModel<TSource>()
            {
                Items = await data.ToListAsync(),
                TotalCount = sourceCount,
                TotalPage = (int)Math.Ceiling((double)sourceCount / pageSize)
            };

        }
    }
}
