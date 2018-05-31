using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class GroupManagerInsertViewModel:BaseInsertViewModel
    {
        //public int? FieldId { get; set; }

        [Required]
        public List<int> FieldsId { get; set; }
    }
}
