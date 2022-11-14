using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using System;
using System.Security;

namespace Security
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTheFramework(this IApplicationBuilder app)
        {
            app.HandleExceptions();
            //app.UseExceptionHandler("/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
            app.UseDeveloperExceptionPage();

            // setup some basics
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // setup OData
            app.UseODataBatching();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.SetIsOriginAllowed(_ => true);
                builder.AllowCredentials();
            });

            app.UseSwaggerUI()
                .UseSwagger()
                .UseODataRouteDebug();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(name: "api", pattern: "api/{controller}/{action}"));

            return app;
        }

        static IApplicationBuilder HandleExceptions(this IApplicationBuilder app)
            => app.UseExceptionHandler(errorApp => errorApp.Run(async (context) =>
            {
                Exception ex = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                context.Response.StatusCode = ex?.GetType() == typeof(SecurityException) ? 401 : 500;
                context.Response.ContentType = "application/json";

                if (ex != null)
                {
                    await context.Response.WriteAsync("{ \"error\": \"" + ex.Message.Replace("\"", "\'") + "\" }");
                    Exception innerEx = ex.InnerException;

                    while (innerEx != null)
                    {
                        innerEx = innerEx.InnerException;
                    }
                }
            }));
    }
}