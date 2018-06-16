using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class ChatGetRepliesViewModel:Pagination
    {
        [Required]
        public int? ChatId { get; set; }

    }
}
