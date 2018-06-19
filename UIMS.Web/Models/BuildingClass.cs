using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class BuildingClass:BaseModelTracker,IEnable
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int BuildingId { get; set; }

        public virtual Building Building { get; set; }


        public virtual ICollection<Presentation> Presentations { get; set; }

        public bool Enable { get; set; }

    }
}
