using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Student:BaseModelTracker,IUser
    {
        [MaxLength(14)]
        public string Code { get; set; }

        public virtual AppUser User { get; set; }

        public int UserId { get; set; }


        public virtual ICollection<StudentPresentation> Classes { get; set; }
    }
}
