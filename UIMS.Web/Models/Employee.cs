using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Employee:BaseModelTracker,IUser
    {
        [MaxLength(80)]
        public string Post { get; set; }


        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
}
