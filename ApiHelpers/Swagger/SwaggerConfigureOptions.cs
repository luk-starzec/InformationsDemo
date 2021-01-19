using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiHelpers.Swagger
{
    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        readonly string apiName;
        public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider, string apiName)
        {
            this.provider = provider;
            this.apiName = apiName;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, apiName));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, string title)
        {
            var info = new OpenApiInfo
            {
                Title = title,
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version is deprecated.";
            }

            return info;
        }
    }
}
