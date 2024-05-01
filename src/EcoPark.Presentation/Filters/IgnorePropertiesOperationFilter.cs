using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EcoPark.Presentation.Filters;

public class IgnorePropertiesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var ignoredProperties = new[] { "RequestUserInfo" };

        foreach (var parameter in operation.Parameters.ToArray())
        {
            if (ignoredProperties.Contains(parameter.Name))
            {
                operation.Parameters.Remove(parameter);
            }
        }
    }
}