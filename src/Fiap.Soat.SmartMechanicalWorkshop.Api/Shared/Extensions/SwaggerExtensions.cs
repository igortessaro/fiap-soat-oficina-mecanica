namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSwaggerGen(options =>
            {
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
                }
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SmartMechanicalWorkshop",
                    Version = "v1",
                    Description = "Veja a documentação no [ReDoc](/docs) <br> Repositorio do projeto: [GitHub](https://github.com/igortessaro/fiap-soat-oficina-mecanica)"
                });
                // Adiciona suporte para o header Authorization
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            return services;
        }
    }
}
