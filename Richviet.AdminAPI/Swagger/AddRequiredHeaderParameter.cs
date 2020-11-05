
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Richviet.Admin.API.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            if (!context.MethodInfo.GetCustomAttributes(true)
              .Any(_ => _ is AllowAnonymousAttribute))
            {

                var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                });
            }


            // Swagger UI不再支援此種方式
            // 參考 github issue https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1425
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Description = "格式如:<br/>Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjExIiwiZW1haWwiOiIiLCJuYW1lIjoiVHLhuqduIMO0bmcgTWluaCIsImNvdW50cnkiOiJUVyIsIm5iZiI6MTYwMzY4NjI3MiwiZXhwIjoxNjAzNjg4MDcyLCJpYXQiOjE2MDM2ODYyNzJ9.1-v_YLr8Rhvp4ZjmJsPmAUaCBbXPJr2wQ97Dea-T4Ik",
            //    Required = true,
            //    Schema = new OpenApiSchema
            //    {
            //        Type = "String",
            //        Default = new OpenApiString("Bearer ")
            //    }
            //});
        }
    }
}
