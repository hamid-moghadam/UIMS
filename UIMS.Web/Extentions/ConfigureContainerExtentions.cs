using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIMS.Web.Data;
using UIMS.Web.Data.AppConfigurations;
using UIMS.Web.Data.Helpers;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Extentions
{
    public static class ConfigureContainerExtentions
    {
        public static void AddDbContext(this IServiceCollection serviceCollection,
            string dataConnectionString = null, string authConnectionString = null)
        {
            //serviceCollection.AddDbContext<DataContext>(options =>
            //    options.UseSqlServer(dataConnectionString ?? GetDataConnectionStringFromConfig()));

            serviceCollection.AddDbContext<DataContext>(options =>
                options.UseNpgsql(dataConnectionString ?? GetDataConnectionStringFromConfig()));

            serviceCollection.AddEntityFrameworkNpgsql();
        }

        public static void AddIdentity(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddCustomizedAuthentication(this IServiceCollection serviceCollection)
        {
            var tokenInfo = GetTokenAuthenticationInfo();

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = tokenInfo.Issuer,
                    ValidAudience = tokenInfo.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenInfo.SecretKey))
                };

            });
        }

        public static void AddCustomizedAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
                options.AddPolicy("supervisor", policy => policy.RequireRole("supervisor", "admin"));
                options.AddPolicy("professor", policy => policy.RequireRole("professor", "admin"));
                options.AddPolicy("student", policy => policy.RequireRole("student", "admin"));
                options.AddPolicy("buildingManager", policy => policy.RequireRole("buildingManager", "admin"));
                options.AddPolicy("groupManager", policy => policy.RequireRole("groupManager", "admin"));
                options.AddPolicy("employee", policy => policy.RequireRole("employee", "admin"));

                options.AddPolicy("all", policy => policy.RequireRole("student", "admin", "professor", "buildingManager", "groupManager", "employee", "supervisor"));
            });
        }

        public static void AddSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                //c.AddSecurityDefinition("",new )
                c.SwaggerDoc("Admin-V1", new Info { Title = "Admin", Version = "v1" });
                c.SwaggerDoc("Supervisor-V1", new Info { Title = "Supervisor", Version = "v1" });
                c.SwaggerDoc("Professor-V1", new Info { Title = "Professor", Version = "v1" });
                c.SwaggerDoc("Student-V1", new Info { Title = "Student", Version = "v1" });
                c.SwaggerDoc("BuildingManager-V1", new Info { Title = "BuildingManager", Version = "v1" });
                c.SwaggerDoc("GroupManager-V1", new Info { Title = "GroupManager", Version = "v1" });
                c.SwaggerDoc("Employee-V1", new Info { Title = "Employee", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
        }

        public static void AddScopedService(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            serviceCollection.AddScoped(typeof(IBaseServiceProvider<>), typeof(BaseServiceProvider<>));
            //test done
            serviceCollection.AddScoped<UserService>();
            serviceCollection.AddScoped<EmployeeService>();
            serviceCollection.AddScoped<BuildingService>();
            //-- test done

            serviceCollection.AddScoped<BuildingManagerService>();
            serviceCollection.AddScoped<DegreeService>();
            serviceCollection.AddScoped<FieldService>();
            serviceCollection.AddScoped<GroupManagerService>();
            serviceCollection.AddScoped<ProfessorService>();
            serviceCollection.AddScoped<SemesterService>();
            serviceCollection.AddScoped<StudentService>();
            serviceCollection.AddScoped<CourseService>();
            serviceCollection.AddScoped<BuildingClassService>();
            serviceCollection.AddScoped<CourseFieldService>();
            serviceCollection.AddScoped<PresentationService>();

        }

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<IBookService, BookService>();

            //serviceCollection.AddTransient<IEmailSender, EmailSender>();
        }

        private static string GetDataConnectionStringFromConfig()
        {
            return new DatabaseConfiguration().GetDataConnectionString();
        }

        private static TokenAuthenticationInfo GetTokenAuthenticationInfo()
        {
            return new TokenConfiguration().GetTokenAuthenticationInfo();
        }
    }
}
