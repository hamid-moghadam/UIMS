using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class SettingsInsertViewModel
    {
        [MaxLength(100)]
        public string AccessName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Value { get; set; }
    }
}
