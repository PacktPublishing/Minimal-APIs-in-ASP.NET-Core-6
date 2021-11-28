using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerSample
{
    public class AnnotationsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            IEnumerable<object> controllerAttributes = Array.Empty<object>();
            IEnumerable<object> actionAttributes = Array.Empty<object>();
            IEnumerable<object> metadataAttributes = Array.Empty<object>();

            if (context.MethodInfo is not null)
            {
                controllerAttributes = context.MethodInfo.DeclaringType!.GetCustomAttributes(true);
                actionAttributes = context.MethodInfo.GetCustomAttributes(true);
            }

            if (context.ApiDescription.ActionDescriptor.EndpointMetadata is not null)
            {
                metadataAttributes = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            }

            // NOTE: When controller and action attributes are applicable, action attributes should take precendence.
            // Hence why they're at the end of the list (i.e. last one wins).
            // Distinct() is applied due to an ASP.NET Core issue: https://github.com/dotnet/aspnetcore/issues/34199.
            var allAttributes = controllerAttributes
                .Union(actionAttributes)
                .Union(metadataAttributes)
                .Distinct();

            var actionAndEndpointAttribtues = actionAttributes
                .Union(metadataAttributes)
                .Distinct();

            ApplySwaggerOperationAttribute(operation, actionAndEndpointAttribtues);
        }

        private static void ApplySwaggerOperationAttribute(
            OpenApiOperation operation,
            IEnumerable<object> actionAttributes)
        {
            var swaggerOperationAttribute = actionAttributes
                .OfType<SwaggerOperationAttribute>()
                .FirstOrDefault();

            if (swaggerOperationAttribute == null)
            {
                return;
            }

            if (swaggerOperationAttribute.Summary != null)
            {
                operation.Summary = swaggerOperationAttribute.Summary;
            }

            if (swaggerOperationAttribute.Description != null)
            {
                operation.Description = swaggerOperationAttribute.Description;
            }

            if (swaggerOperationAttribute.OperationId != null)
            {
                operation.OperationId = swaggerOperationAttribute.OperationId;
            }

            if (swaggerOperationAttribute.Tags != null)
            {
                operation.Tags = swaggerOperationAttribute.Tags
                    .Select(tagName => new OpenApiTag { Name = tagName })
                    .ToList();
            }
        }
    }
}
