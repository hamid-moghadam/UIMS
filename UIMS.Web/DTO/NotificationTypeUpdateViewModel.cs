﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class NotificationTypeUpdateViewModel : BaseModel
    {
        public string Type { get; set; }

        public int? Priority { get; set; }

    }
}
