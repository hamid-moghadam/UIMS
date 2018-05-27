using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UIMS.Web.Data.ModelConfigurations
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder
                .HasOne(x => x.BuildingManager)
                .WithOne(x => x.Building)
                .HasForeignKey<BuildingManager>(b=>b.BuildingId);
        }
    }
}
