using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class BaseUpdateViewModel:BaseInsertViewModel
    {
        [MaxLength(11)]
        public string UserPhoneNumber { get; set; }
    }
}
