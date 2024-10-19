using LightNap.Core;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.Migrations.SqlServer.Extensions;
using LightNap.WebApi.Configuration;
using LightNap.WebApi.Extensions;
using LightNap.WebApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));
builder.Services.Configure<List<AdministratorConfiguration>>(builder.Configuration.GetSection("Administrators"));

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions((options) =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Select a DB provider. Ensure you reference the appropriate library and update appsettings.config if necessary.
builder.Services.AddLightNapSqlServer(builder.Configuration);

// Select an email provider. Ensure you reference the appropriate library and update appsettings.config if necessary.
builder.Services.AddLogToConsoleEmailer();
//builder.Services.AddSmtpEmailer();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCors(builder =>
    builder
        .WithOrigins("https://localhost:4200", "http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    var administratorSettings = services.GetRequiredService<IOptions<List<AdministratorConfiguration>>>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    await context.Database.MigrateAsync();
    await Seeder.SeedRoles(roleManager, logger);
    await Seeder.SeedAdministrators(userManager, roleManager, administratorSettings, logger);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    if (logger is not null)
    {
        logger.LogError(ex, "An error occurred during migration and/or seeding");
    }
    else
    {
        Trace.TraceError($"A logger was not available to report startup error: {ex}");
    }
}

app.Run();
