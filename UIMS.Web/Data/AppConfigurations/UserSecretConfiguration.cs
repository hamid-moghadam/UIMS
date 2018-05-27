using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UIMS.Web.Data.AppConfigurations
{
    public class UserSecretConfiguration:ConfigurationBase
    {
        public void AddUserSecret(UserSecret userSecret)
        {
            GetConfigurationBuilder().AddUserSecrets(userSecret.University).Build();
        }


    }
}
