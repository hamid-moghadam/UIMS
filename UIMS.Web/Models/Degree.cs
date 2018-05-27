using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class Degree:BaseModelTracker
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
    }
}
