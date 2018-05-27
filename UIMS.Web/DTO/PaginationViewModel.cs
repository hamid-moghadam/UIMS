﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class PaginationViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int TotalPage { get; set; }
    }
}
