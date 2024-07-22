using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Api.StartUps;

public class SwaggerStartUp : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(setup =>
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                BearerFormat = "JWT",
                Name = "AuthToken",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Auth Token",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 9999;
}