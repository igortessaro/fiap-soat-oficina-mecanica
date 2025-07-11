using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);



_ = builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext());

_ = builder.Logging.ClearProviders();
_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddSwaggerGen(options =>
{
    string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (string xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
    }

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SmartMechanicalWorkshop",
        Version = "v1",
        Description = "Veja a documentação no [ReDoc](/docs) <br> Repositorio do projeto: [GitHub](https://github.com/igortessaro/fiap-soat-oficina-mecanica)"
    });
});


_ = builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

_ = builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DbConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbConnectionString")),
        mySqlOptions =>
            mySqlOptions.MigrationsAssembly("Fiap.Soat.SmartMechanicalWorkshop.Infrastructure")
    ));

_ = builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
_ = builder.Services.AddServiceExtensions();
_ = builder.Services.AddRepositoryExtensions();
_ = builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
_ = builder.Services.AddHttpContextAccessor();
_ = builder.Services.AddHealthChecks();
_ = builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    dbContext.Database.EnsureCreated();
}

_ = app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.EnableTryItOutByDefault();
    c.DisplayRequestDuration();
});

app.UseReDoc(c =>
{
    c.RoutePrefix = "docs";
    c.DocumentTitle = "Smart Mechanical Workshop API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});
_ = app.UseMiddleware<ExceptionMiddleware>();
_ = app.UseHttpsRedirection();
_ = app.UseAuthorization();
_ = app.MapControllers();

try
{
    Log.Information("Starting up...");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}


