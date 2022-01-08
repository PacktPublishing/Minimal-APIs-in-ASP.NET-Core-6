using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerSample.Filter;

public class CorrelationIdOperationFilter : IOperationFilter
{
    private readonly IWebHostEnvironment environment;
    public CorrelationIdOperationFilter(IWebHostEnvironment environment)
    {
        this.environment = environment;
    }

    /// <summary>
    /// Apply header in parameter Swagger.
    /// We add default value in parameter for developer environment
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        if (operation.OperationId == "SampleResponseOperation")
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-correlation-id",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString("42") }
            });
        }

    }
}

