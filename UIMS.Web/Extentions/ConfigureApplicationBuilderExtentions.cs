using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Extentions
{
    public static class ConfigureApplicationBuilderExtentions
    {

        private static readonly string Auth_Query_String_Key = "Access_Token";


        public static void HandleSignalRWithJWT(this IApplicationBuilder app)
        {
            app.Use(async (Mcontext, next) => {
                if (Mcontext.Request.Query.TryGetValue(Auth_Query_String_Key, out var token))
                {
                    Mcontext.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
                await next();

            });
        }


        public static void HandleApiRequests(this IApplicationBuilder app)
        {
            app.Use(async (Mcontext, next) => {
                await next();
                if (Mcontext.Response.StatusCode == 404 &&
                !Path.HasExtension(Mcontext.Request.Path.Value) &&
                !Mcontext.Request.Path.Value.StartsWith("/api"))
                {
                    Mcontext.Request.Path = "/admin/index.html";
                    await next();
                }
            });



        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";

            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Admin-V1/swagger.json", "Admin-V1");
                c.SwaggerEndpoint("/swagger/Supervisor-V1/swagger.json", "Supervisor-V1");
                c.SwaggerEndpoint("/swagger/Professor-V1/swagger.json", "Professor-V1");
                c.SwaggerEndpoint("/swagger/Student-V1/swagger.json", "Student-V1");
                c.SwaggerEndpoint("/swagger/BuildingManager-V1/swagger.json", "BuildingManager-V1");
                c.SwaggerEndpoint("/swagger/GroupManager-V1/swagger.json", "GroupManager-V1");
                c.SwaggerEndpoint("/swagger/Employee-V1/swagger.json", "Employee-V1");
            });
        }
    }
}
