using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Security.Api.OData;
using Security.Data.EF;
using Security.Data.EF.MSSQL;

namespace SecuritySQLite
{
    public static partial class IServiceCollectionExtensions
    {
        public static void AddAspNetCore(this IServiceCollection services)
        {
            _ = services.AddResponseCompression();
            _ = services.AddMvcCore(options =>
            {
                options.MaxIAsyncEnumerableBufferLimit = int.MaxValue;
                options.MaxModelBindingCollectionSize = 10000;
                options.MaxModelBindingRecursionDepth = 10;
            })
                .AddDataAnnotations()
                .AddCors(options => options.AddDefaultPolicy(builder =>
                {
                    _ = builder.AllowAnyHeader();
                    _ = builder.AllowAnyMethod();
                    _ = builder.SetIsOriginAllowed(origin => true);
                    _ = builder.AllowCredentials();
                }));

            services.AddControllers()
               .AddOData(opt =>
               {
                   opt.RouteOptions.EnableQualifiedOperationCall = false;
                   opt.EnableAttributeRouting = true;
                   _ = opt.Expand().Count().Filter().Select().OrderBy().SetMaxTop(1000);
                   opt.AddRouteComponents($"/Api/Security", new SecurityModelBuilder().Build().EDMModel);
               });
        }

        public static void AddMetadata(this IServiceCollection services)
        {
            _ = services.AddEndpointsApiExplorer();

            _ = services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = @"Authorization header using the Bearer scheme. \r\n\r\n 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });
            });
        }
    }
}