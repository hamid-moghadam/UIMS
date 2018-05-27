using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models
{
    public class StudentPresentation:BaseModelTracker
    {

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }



        public int PresentationId { get; set; }

        public virtual Presentation Presentation { get; set; }
    }
}
