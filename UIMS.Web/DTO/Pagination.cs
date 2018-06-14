using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class Pagination
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}
