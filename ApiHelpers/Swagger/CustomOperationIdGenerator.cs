using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;

namespace ApiHelpers.Swagger
{
    public static class CustomOperationIdGenerator
    {
        public static string GetOperationId(ApiDescription apiDesc)
        {
            var controler = ((ControllerActionDescriptor)apiDesc.ActionDescriptor).ControllerName;

            var customName = apiDesc.ActionDescriptor.AttributeRouteInfo.Name;
            if (!string.IsNullOrEmpty(customName))
                return $"{controler}_{customName}";

            var action = ((ControllerActionDescriptor)apiDesc.ActionDescriptor).ActionName;
            var parameters = apiDesc.ActionDescriptor.Parameters;

            var name = $"{controler}_{action}";
            if (parameters.Any())
                name = $"{name}_{string.Join("_", parameters.Select(r => r.Name))}";

            return name;
        }
    }
}
