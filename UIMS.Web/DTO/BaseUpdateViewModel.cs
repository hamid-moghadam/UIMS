﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class BaseUpdateViewModel:BaseModel
    {
        [Number]
        [StringLength(11,MinimumLength =11)]
        public string PhoneNumber { get; set; }

        //[Number]
        //[StringLength(10, MinimumLength = 10)]
        //public string MelliCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(80)]
        public string Family { get; set; }
    }
}
