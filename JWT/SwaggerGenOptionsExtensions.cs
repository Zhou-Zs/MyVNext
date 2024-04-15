using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace JWT
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void AddAuthenticationHeader(this SwaggerGenOptions c)
        {
            // 添加一个或多个“securityDefinitions”，描述如何保护您的 API， 到生成的 Swagger;Authorization 是方案名称
            c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
            {
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Authorization"
            });

            // 添加全局安全要求
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() 
            {
                {
                     new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                     new List<string>()
                }
            });
        }
    }
}
