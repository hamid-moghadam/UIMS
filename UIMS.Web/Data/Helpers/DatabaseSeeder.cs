using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.Data.Helpers
{
    public class DatabaseSeeder
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;


        public DatabaseSeeder(DataContext dataContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<int> SeedUserEntities()
        {
            AppUser hamid = new AppUser()
            {
                Name = "حمید",
                Family = "مقدم",
                UserName = "hamid",
                MelliCode = "6191234566",
                PhoneNumber = "09393615326",
            };

            AppUser moein = new AppUser()
            {
                Name = "معین",
                Family = "زارع",
                UserName = "moein",
                MelliCode = "1256398562",
                PhoneNumber = "09137538515",
            };

            AppUser groupManager = new AppUser()
            {
                Name = "قاسم",
                Family = "آذری",
                UserName = "azari",
                PhoneNumber = "09312563589",
                MelliCode = "6198523617",
                GroupManager = new GroupManager()
                {
                    
                },
                Professor = new Professor()
                {
                    
                }
            };

            await _userManager.CreateAsync(hamid, "H@mid0077");
            await _userManager.CreateAsync(moein, "MoE!n0078");
            await _userManager.CreateAsync(groupManager, "azari");

            await _dataContext.SaveChangesAsync();

            await _userManager.AddToRoleAsync(hamid, "admin");
            await _userManager.AddToRoleAsync(moein, "admin");
            await _userManager.AddToRolesAsync(groupManager, new List<string>() { "groupManager","professor" });

            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> SeedRoleEnities()
        {
            var roles = new List<AppRole>()
            {
                new AppRole(){Name = "admin"},
                new AppRole(){Name = "supervisor"},
                new AppRole(){Name = "student"},
                new AppRole(){Name = "professor"},
                new AppRole(){Name = "buildingManager"},
                new AppRole(){Name = "groupManager"},
                new AppRole(){Name = "employee"},

            };
            roles.ForEach(x => _roleManager.CreateAsync(x).Wait());
            return await _dataContext.SaveChangesAsync();
        }
    }
}
