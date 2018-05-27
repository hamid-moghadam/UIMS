using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Professor: BaseModelTracker, IUser
    {
        public virtual AppUser User { get; set; }

        public int UserId { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}
