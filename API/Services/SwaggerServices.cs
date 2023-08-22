using Microsoft.OpenApi.Models;

namespace API.Services
{
    public static class SwaggerServices
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Basic",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Basic",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Basic authorization header",
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Basic"
                                }
                            },
                            new string[] { "Basic " }
                        }
                    }
                );
            });

            return services;
        }
    }
}
