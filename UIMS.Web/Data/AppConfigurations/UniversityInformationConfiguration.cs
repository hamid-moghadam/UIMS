using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Data.Helpers;

namespace UIMS.Web.Data.AppConfigurations
{
    public class UniversityInformationConfiguration:ConfigurationBase
    {
        public UniversityInfo GetUniversityInfo()
        {
            return new UniversityInfo()
            {
                Name = GetConfiguration().GetSection("UniversityInfo:Name").Value
            };
        }
    }
}
