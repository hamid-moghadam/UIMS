using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Data.AppConfigurations
{
    public class DatabaseConfiguration : ConfigurationBase
    {
        private string DataConnectionKey = "UIMDataConnection";

        private string AuthConnectionKey = "UIMAuthConnection";

        public string GetDataConnectionString()
        {
            return GetConfiguration().GetConnectionString(DataConnectionKey);
        }

        public string GetAuthConnectionString()
        {
            return GetConfiguration().GetConnectionString(AuthConnectionKey);
        }
    }
}
