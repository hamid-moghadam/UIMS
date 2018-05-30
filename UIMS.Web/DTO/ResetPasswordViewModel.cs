using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Username { get; set; }


        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
