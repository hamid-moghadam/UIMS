using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIMS.Web.Data.Extentions;
using UIMS.Web.Data.ModelConfigurations;
using UIMS.Web.Models;

namespace UIMS.Web.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Building> Building { get; set; }
        public DbSet<BuildingClass> BuildingClass { get; set; }
        public DbSet<BuildingManager> BuildingManager { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseField> CourseField { get; set; }
        public DbSet<Degree> Degree { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Field> Field { get; set; }
        public DbSet<GroupManager> GroupManager { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationReceiver> NotificationReceiver { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<Professor> Professor { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentPresentation> StudentPresentation { get; set; }
        public DbSet<AppUser> User { get; set; }
        public DbSet<Settings> Settings { get; set; }



        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyTrackingInformation();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            ChangeTracker.ApplyTrackingInformation();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyTrackingInformation();
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.ApplyTrackingInformation();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new PresentationConfiguration());
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new BuildingConfiguration());

            base.OnModelCreating(builder);
        }


    }
}
