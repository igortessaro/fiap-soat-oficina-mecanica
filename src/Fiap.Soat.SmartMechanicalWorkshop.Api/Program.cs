using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Adapters;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.HealthChecks;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Logging;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Mappers;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Shared.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

_ = builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.With(new ServiceOrderEnricher(services.GetRequiredService<IHttpContextAccessor>())));

_ = builder.Logging.ClearProviders();
_ = builder.Services.AddControllers();
_ = builder.Services.AddEndpointsApiExplorer();
_ = builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

_ = builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")),
        mySqlOptions =>
            mySqlOptions.MigrationsAssembly("Fiap.Soat.SmartMechanicalWorkshop.Infrastructure")
    ));

_ = builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
_ = builder.Services.AddServiceExtensions();
_ = builder.Services.AddRepositoryExtensions();
_ = builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
_ = builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateServiceOrderStatusCommand).Assembly));
_ = builder.Services.AddHttpContextAccessor();

_ = builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(name: "database", tags: ["db", "mysql"])
    .AddCheck<MemoryHealthCheck>("memory", tags: ["memory", "resources"]);

_ = builder.Services.AddRouting(options => options.LowercaseUrls = true);
_ = builder.Services.AddAuthenticationExtension(builder.Configuration);
_ = builder.Services.AddSwaggerExtension(builder.Configuration);
_ = builder.Services.AddMemoryCache();
_ = builder.Services.AddInterfaceAdapters();

_ = builder.Services.AddSingleton<INewRelicTelemetryService, NewRelicTelemetryService>();
_ = builder.Services.AddSingleton<ITelemetryService, NewRelicTelemetryAdapter>();

var app = builder.Build();

_ = app.UseSwagger();
_ = app.UseSwaggerUI(c =>
{
    c.EnableTryItOutByDefault();
    c.DisplayRequestDuration();
});

_ = app.UseReDoc(c =>
{
    c.RoutePrefix = "docs";
    c.DocumentTitle = "Smart Mechanical Workshop API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});
_ = app.UseMiddleware<ExceptionMiddleware>();
_ = app.UseHttpsRedirection();
_ = app.UseAuthorization();
_ = app.MapControllers();

_ = app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckResponseWriter.WriteResponse
});

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
