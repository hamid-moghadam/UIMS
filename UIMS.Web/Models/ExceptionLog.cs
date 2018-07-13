using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Models
{
    public class ExceptionLog : BaseModelTracker
    {
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string HelpLink { get; set; }
        public Exception InnerException { get; set; }
    }
}
