using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class ExceptionLogViewModel:BaseModel
    {
        public string StackTrace { get; }
        public string Source { get; set; }
        public string Message { get; }
        public string HelpLink { get; set; }
        public Exception InnerException { get; }
    }
}
