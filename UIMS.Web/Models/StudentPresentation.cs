using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class StudentPresentation:BaseModelTracker,IEnable
    {

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }



        public int PresentationId { get; set; }

        public virtual Presentation Presentation { get; set; }


        public bool Enable { get; set; }
    }
}
