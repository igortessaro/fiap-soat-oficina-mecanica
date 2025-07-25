using Fiap.Soat.MechanicalWorkshop.Application.Commands;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Middlewares;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

_ = builder.Host.UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext());

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
        builder.Configuration.GetConnectionString("DbConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbConnectionString")),
        mySqlOptions =>
            mySqlOptions.MigrationsAssembly("Fiap.Soat.SmartMechanicalWorkshop.Infrastructure")
    ));

_ = builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
_ = builder.Services.AddServiceExtensions();
_ = builder.Services.AddRepositoryExtensions();
_ = builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
_ = builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceOrderChangeStatusCommand).Assembly));
_ = builder.Services.AddHttpContextAccessor();
_ = builder.Services.AddHealthChecks();
_ = builder.Services.AddRouting(options => options.LowercaseUrls = true);
_ = builder.Services.AddAuthenticationExtension(builder.Configuration);
_ = builder.Services.AddSwaggerExtension(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    dbContext.Database.EnsureCreated();
}

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

await app.RunAsync();

public partial class Program { }
