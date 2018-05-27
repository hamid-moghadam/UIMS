using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UIMS.Web.Data.ModelConfigurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
