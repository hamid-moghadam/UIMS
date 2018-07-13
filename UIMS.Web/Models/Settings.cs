using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Models
{
    public class Settings:BaseModelTracker
    {
        [MaxLength(100)]
        public string AccessName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Value { get; set; }

    }
}
