using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

public class ProducesResponseTypeAttributeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodAttributes = context.MethodInfo.GetCustomAttributes(true)
            .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true))
            .OfType<ProducesResponseTypeAttribute>();

        foreach (var attr in methodAttributes)
        {
            var statusCode = attr.StatusCode.ToString();

            if (!operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses.Add(statusCode, new OpenApiResponse
                {
                    Description = attr.Type == null ? "No additional information" : attr.Type.Name
                });
            }
        }
    }
}