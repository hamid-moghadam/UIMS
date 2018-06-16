using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class ChatInsertViewModel
    {
        [Required]
        public int? FirstId { get; set; }

        [Required]
        public int? SecondId { get; set; }

    }
}
