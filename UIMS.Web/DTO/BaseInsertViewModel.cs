using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class BaseInsertViewModel:BaseModel
    {
        [MaxLength(10)]
        public string UserMelliCode { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(80)]
        public string UserFamily { get; set; }

    }
}
