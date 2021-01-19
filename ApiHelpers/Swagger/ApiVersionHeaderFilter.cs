using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ApiHelpers.Swagger
{
    public class ApiVersionHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var name = "X-Version";

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var group = context.ApiDescription.GroupName;
            var version = !string.IsNullOrEmpty(group) ? group.Replace("v", "") : "1";

            var parameter = operation.Parameters.Where(r => r.Name == name).FirstOrDefault();

            if (parameter != null)
            {
                parameter.Required = true;
                parameter.Schema.Default = new OpenApiString(version);
            }
            else
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = name,
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString(version) },
                });
            }
        }
    }
}
