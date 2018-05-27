using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UIMS.Web.Data.ModelConfigurations
{
    public class PresentationConfiguration : IEntityTypeConfiguration<Presentation>
    {
        public void Configure(EntityTypeBuilder<Presentation> builder)
        {
            builder
                .HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
