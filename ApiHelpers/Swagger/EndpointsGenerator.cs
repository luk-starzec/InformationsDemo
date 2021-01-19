using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Linq;

namespace ApiHelpers.Swagger
{
    public static class EndpointsGenerator
    {
        public static SwaggerUIOptions AddEndpoints(this SwaggerUIOptions options, string apiName, IApiVersionDescriptionProvider provider)
        {
            foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(r => r.ApiVersion))
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{apiName} {description.GroupName}");
            }
            return options;
        }

    }
}
