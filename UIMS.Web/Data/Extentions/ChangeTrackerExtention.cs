using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Data.Extentions
{
    public static class ChangeTrackerExtention
    {
        public static void ApplyTrackingInformation(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (!(entry.Entity is ITracker baseAudit)) continue;
                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        //baseAudit.Created = now;
                        baseAudit.Modified = now;
                        break;

                    case EntityState.Added:
                        if (entry.Entity is IEnable baseEnable && entry.Entity.GetType() != typeof(Semester))
                            baseEnable.Enable = true;
                        baseAudit.Created = now;
                        break;
                }
            }

        }
    }
}
