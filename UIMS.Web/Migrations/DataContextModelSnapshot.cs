﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UIMS.Web.Data;

namespace UIMS.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("UIMS.Web.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<string>("PersianName")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("UIMS.Web.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("BuildingManagerId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("EmployeeId");

                    b.Property<bool>("Enable");

                    b.Property<string>("Family")
                        .HasMaxLength(80);

                    b.Property<int>("GroupManagerId");

                    b.Property<DateTime?>("LastLogin");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MelliCode")
                        .HasMaxLength(10);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("ProfessorId");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("StudentId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("MelliCode")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("UIMS.Web.Models.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuildingManagerId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Building");
                });

            modelBuilder.Entity("UIMS.Web.Models.BuildingClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuildingId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("BuildingClass");
                });

            modelBuilder.Entity("UIMS.Web.Models.BuildingManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuildingId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("BuildingManager");
                });

            modelBuilder.Entity("UIMS.Web.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<int>("FirstUserId");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("SecondUserId");

                    b.HasKey("Id");

                    b.HasIndex("FirstUserId");

                    b.HasIndex("SecondUserId");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("UIMS.Web.Models.ChatReply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("HasSeen");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("ReplierId");

                    b.Property<string>("Reply")
                        .HasMaxLength(1000);

                    b.Property<int>("SemesterId");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("ReplierId");

                    b.HasIndex("SemesterId");

                    b.ToTable("ChatReply");
                });

            modelBuilder.Entity("UIMS.Web.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Course");
                });

            modelBuilder.Entity("UIMS.Web.Models.CourseField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<int>("FieldId");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("FieldId");

                    b.ToTable("CourseField");
                });

            modelBuilder.Entity("UIMS.Web.Models.Degree", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Degree");
                });

            modelBuilder.Entity("UIMS.Web.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Post")
                        .HasMaxLength(80);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("UIMS.Web.Models.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<int>("DegreeId");

                    b.Property<int?>("GroupManagerId");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("DegreeId");

                    b.HasIndex("GroupManagerId");

                    b.ToTable("Field");
                });

            modelBuilder.Entity("UIMS.Web.Models.GroupManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("GroupManager");
                });

            modelBuilder.Entity("UIMS.Web.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NotificationTypeId");

                    b.Property<int>("SemesterId");

                    b.Property<int>("SenderId");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(200);

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("SemesterId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("UIMS.Web.Models.NotificationAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AppRoleId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NotificationTypeId");

                    b.HasKey("Id");

                    b.HasIndex("AppRoleId");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("NotificationAccess");
                });

            modelBuilder.Entity("UIMS.Web.Models.NotificationReceiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("HasSeen");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NotificationId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("NotificationReceiver");
                });

            modelBuilder.Entity("UIMS.Web.Models.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("Priority");

                    b.Property<string>("Type")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NotificationType");
                });

            modelBuilder.Entity("UIMS.Web.Models.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuildingClassId");

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<int>("CourseFieldId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("Day");

                    b.Property<bool>("Enable");

                    b.Property<string>("End")
                        .HasMaxLength(5);

                    b.Property<DateTime>("Modified");

                    b.Property<int>("ProfessorId");

                    b.Property<int>("SemesterId");

                    b.Property<string>("Start")
                        .HasMaxLength(5);

                    b.HasKey("Id");

                    b.HasIndex("BuildingClassId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CourseFieldId");

                    b.HasIndex("ProfessorId");

                    b.HasIndex("SemesterId");

                    b.ToTable("Presentation");
                });

            modelBuilder.Entity("UIMS.Web.Models.Professor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Professor");
                });

            modelBuilder.Entity("UIMS.Web.Models.Semester", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.ToTable("Semester");
                });

            modelBuilder.Entity("UIMS.Web.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessName")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("UIMS.Web.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(14);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Student");
                });

            modelBuilder.Entity("UIMS.Web.Models.StudentPresentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Enable");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("PresentationId");

                    b.Property<int>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("PresentationId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentPresentation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.BuildingClass", b =>
                {
                    b.HasOne("UIMS.Web.Models.Building", "Building")
                        .WithMany("BuildingClasses")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.BuildingManager", b =>
                {
                    b.HasOne("UIMS.Web.Models.Building", "Building")
                        .WithOne("BuildingManager")
                        .HasForeignKey("UIMS.Web.Models.BuildingManager", "BuildingId");

                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithOne("BuildingManager")
                        .HasForeignKey("UIMS.Web.Models.BuildingManager", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Chat", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.AppUser", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.ChatReply", b =>
                {
                    b.HasOne("UIMS.Web.Models.Chat", "Chat")
                        .WithMany("Replies")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.AppUser", "Replier")
                        .WithMany("ConversationReplies")
                        .HasForeignKey("ReplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Semester", "Semester")
                        .WithMany("ConversationReplies")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.CourseField", b =>
                {
                    b.HasOne("UIMS.Web.Models.Course", "Course")
                        .WithMany("Fields")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Field", "Field")
                        .WithMany("Courses")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Employee", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithOne("Employee")
                        .HasForeignKey("UIMS.Web.Models.Employee", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Field", b =>
                {
                    b.HasOne("UIMS.Web.Models.Degree", "Degree")
                        .WithMany("Fields")
                        .HasForeignKey("DegreeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.GroupManager", "GroupManager")
                        .WithMany("Fields")
                        .HasForeignKey("GroupManagerId");
                });

            modelBuilder.Entity("UIMS.Web.Models.GroupManager", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithOne("GroupManager")
                        .HasForeignKey("UIMS.Web.Models.GroupManager", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Notification", b =>
                {
                    b.HasOne("UIMS.Web.Models.NotificationType", "NotificationType")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Semester", "Semester")
                        .WithMany("Messages")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.AppUser", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.NotificationAccess", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppRole", "AppRole")
                        .WithMany("NotificationTypes")
                        .HasForeignKey("AppRoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.NotificationType", "NotificationType")
                        .WithMany("Roles")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.NotificationReceiver", b =>
                {
                    b.HasOne("UIMS.Web.Models.Notification", "Notification")
                        .WithMany("Receivers")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Presentation", b =>
                {
                    b.HasOne("UIMS.Web.Models.BuildingClass", "BuildingClass")
                        .WithMany("Presentations")
                        .HasForeignKey("BuildingClassId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.CourseField", "CourseField")
                        .WithMany("Presentations")
                        .HasForeignKey("CourseFieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Professor", "Professor")
                        .WithMany("Presentations")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Semester", "Semester")
                        .WithMany("Presentations")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Professor", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithOne("Professor")
                        .HasForeignKey("UIMS.Web.Models.Professor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.Student", b =>
                {
                    b.HasOne("UIMS.Web.Models.AppUser", "User")
                        .WithOne("Student")
                        .HasForeignKey("UIMS.Web.Models.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UIMS.Web.Models.StudentPresentation", b =>
                {
                    b.HasOne("UIMS.Web.Models.Presentation", "Presentation")
                        .WithMany("Students")
                        .HasForeignKey("PresentationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("UIMS.Web.Models.Student", "Student")
                        .WithMany("Classes")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
