using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class Semester : BaseModelTracker
    {
        [MaxLength(6)]
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}
