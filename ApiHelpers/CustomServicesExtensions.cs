using ApiHelpers.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiHelpers
{
    public static class CustomServicesExtensions
    {
        public static IServiceCollection AddCustomVersioning(this IServiceCollection services)
        {
            services
                .AddApiVersioning(cfg =>
                {
                    cfg.DefaultApiVersion = new ApiVersion(1, 0);
                    cfg.AssumeDefaultVersionWhenUnspecified = true;
                    cfg.ReportApiVersions = true;
                    cfg.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-Version"));
                })
                .AddVersionedApiExplorer(cfg =>
                {
                    cfg.GroupNameFormat = "'v'VVV";
                });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string apiName)
        {
            services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>(s =>
                    new SwaggerConfigureOptions(s.GetRequiredService<IApiVersionDescriptionProvider>(), apiName))
                .AddSwaggerGen(cfg =>
                {
                    cfg.OperationFilter<ApiVersionHeaderFilter>();
                    cfg.CustomOperationIds(apiDesc => CustomOperationIdGenerator.GetOperationId(apiDesc));
                    cfg.UseAllOfToExtendReferenceSchemas();
                });

            return services;
        }


    }
}
