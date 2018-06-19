using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class GroupManagerViewModel:BaseViewModel
    {
        public List<FieldPartialViewModel> Fields { get; set; }
    }
}
