using ApiHelpers.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiHelpers
{
    public static class CustomApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, string apiName, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.AddEndpoints(apiName, provider);
                   c.DisplayOperationId();
                   c.DisplayRequestDuration();
               });

            return app;
        }
    }
}
