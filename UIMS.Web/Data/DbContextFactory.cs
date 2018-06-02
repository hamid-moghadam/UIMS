using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using UIMS.Web.Data.AppConfigurations;

namespace UIMS.Web.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private static string DataConnectionString => new DatabaseConfiguration().GetDataConnectionString();

        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            //optionsBuilder.UseSqlServer(DataConnectionString);

            optionsBuilder.UseNpgsql(DataConnectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
