using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UIMS.Web.Data.ModelConfigurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasIndex(x => x.Code)
                .IsUnique();

            //builder.
            //    HasOne(x => x.User)
            //    .WithOne(x => x.Student)
            //    .HasForeignKey<AppUser>(x => x.StudentId);
        }
    }
}
