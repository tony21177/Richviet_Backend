
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

namespace Richviet.API.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "格式如:<br/>Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjExIiwiZW1haWwiOiIiLCJuYW1lIjoiVHLhuqduIMO0bmcgTWluaCIsImNvdW50cnkiOiJUVyIsIm5iZiI6MTYwMzY4NjI3MiwiZXhwIjoxNjAzNjg4MDcyLCJpYXQiOjE2MDM2ODYyNzJ9.1-v_YLr8Rhvp4ZjmJsPmAUaCBbXPJr2wQ97Dea-T4Ik",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString("Bearer ")
                }
            });
        }
    }
}
