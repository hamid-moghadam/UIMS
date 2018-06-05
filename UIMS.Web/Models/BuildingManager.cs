using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class BuildingManager: BaseModelTracker,IUser
    {
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

        public int? BuildingId { get; set; }

        public virtual Building Building { get; set; }
    }
}
