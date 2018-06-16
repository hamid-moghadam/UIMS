using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.Data.Extentions
{
    public static class CustomEnableExtention
    {
        public static List<Type> TrueEnableTypes => new List<Type>()
        {
            typeof(Semester),
            typeof(Notification),
            typeof(Chat),
        };

    }
}
