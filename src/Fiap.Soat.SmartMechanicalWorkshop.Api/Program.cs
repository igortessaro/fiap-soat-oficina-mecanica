using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Mappings;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Serilog;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api
{
    public class Program
    {
        private static void Main(string[] args)
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
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "te",
                    Version = "V1"
                });
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("DbConnectionString"), new MySqlServerVersion(new Version(8, 4, 5)));
            }
              );

            builder.Services.AddServiceExtensions();
            builder.Services.AddRepositoryExtensions();
            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHealthChecks();

            WebApplication app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
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
}