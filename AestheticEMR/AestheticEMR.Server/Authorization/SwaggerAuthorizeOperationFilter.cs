// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AestheticEMR.Server.Authorization
{
    // Swagger IOperationFilter implementation that will decide which api action needs authorization
    internal class SwaggerAuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Any();

            if (hasAuthorize == true)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

                var oAuthScheme = new OpenApiSecuritySchemeReference("oauth2", new OpenApiDocument(), string.Empty);

                operation.Security =
                [
                    new() { [oAuthScheme] = [] }
                ];
            }
        }
    }
}
