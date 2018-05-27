using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class Building:BaseModelTracker
    {
        [MaxLength(100)]
        public string Name { get; set; }


        public virtual BuildingManager BuildingManager { get; set; }
        public int BuildingManagerId { get; set; }

        public virtual ICollection<BuildingClass> BuildingClasses { get; set; }

    }
}
