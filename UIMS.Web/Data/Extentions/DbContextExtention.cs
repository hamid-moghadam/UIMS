using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIMS.Web.Data.Helpers;
using UIMS.Web.Models;

namespace UIMS.Web.Data.Extentions
{
    public static class DbContextExtensions
    {
        public static int EnsureSeedData(IServiceProvider serviceProvider)
        {
            var userCount = default(int);
            var roleCount = default(int);


            var context = serviceProvider.GetRequiredService<DataContext>();
            //context.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();


            var dbSeeder = new DatabaseSeeder(context,userManager,roleManager);

            if (!roleManager.Roles.Any())
            {
                roleCount = dbSeeder.SeedRoleEnities().Result;
            }

            if (!context.User.Any())
            {
                userCount = dbSeeder.SeedUserEntities().Result;
            }

            return userCount + roleCount;
        }
    }
}
