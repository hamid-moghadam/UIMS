using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class NotificationInsertViewModel
    {
        public int SenderId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Subtitle { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public int SemesterId { get; set; }

        public int MessageTypeId { get; set; }

        public List<NotificationReceiver> Receivers { get; set; }


    }
}
