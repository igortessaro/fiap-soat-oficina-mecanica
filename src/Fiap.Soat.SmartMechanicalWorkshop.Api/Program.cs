using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api;

public class Program
{
    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        Console.WriteLine(JsonSerializer.Serialize($"DbConnectionString: {builder.Configuration.GetConnectionString("DbConnectionString")}, ENVIRONMENT: {builder.Configuration.GetValue<string>("ENVIRONMENT")}"));
        builder.Host.UseSerilog((context, services, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)

        );

        builder.Logging.ClearProviders();


        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
            }

            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "SmartMechanicalWorkshop",
                Version = "V1"
            });
        });

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DbConnectionString"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbConnectionString")),
                mySqlOptions =>
                    mySqlOptions.MigrationsAssembly("Fiap.Soat.SmartMechanicalWorkshop.Infrastructure")
            ));

        builder.Services.AddServiceExtensions();
        builder.Services.AddRepositoryExtensions();
        builder.Services.AddLogging();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await dbContext.Database.MigrateAsync();
                dbContext.Database.EnsureCreated();
            }
        }



        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
