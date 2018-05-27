using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class BaseModelTracker:BaseModel,ITracker
    {
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
