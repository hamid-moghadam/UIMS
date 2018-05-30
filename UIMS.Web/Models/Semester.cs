using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Semester : BaseModelTracker,IEnable
    {
        [MaxLength(6)]
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
        public bool Enable { get ; set ; }
    }
}
