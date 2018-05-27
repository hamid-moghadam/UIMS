using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UIMS.Web.Data.ModelConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasIndex(x => x.MelliCode)
                .IsUnique();

            builder
                .HasOne(x => x.Student)
                .WithOne(x => x.User)
                .HasForeignKey<Student>(x => x.UserId);

            builder
                .HasOne(x => x.Professor)
                .WithOne(x => x.User)
                .HasForeignKey<Professor>(x => x.UserId);

            builder
                .HasOne(x => x.BuildingManager)
                .WithOne(x => x.User)
                .HasForeignKey<BuildingManager>(x => x.UserId);

            builder
                .HasOne(x => x.GroupManager)
                .WithOne(x => x.User)
                .HasForeignKey<GroupManager>(x => x.UserId);


            builder
                .HasOne(x => x.Employee)
                .WithOne(x => x.User)
                .HasForeignKey<Employee>(x => x.UserId);
        }
    }
}
