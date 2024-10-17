using LightNap.MaintenanceService;
using LightNap.MaintenanceService.Tasks;
using LightNap.Migrations.SqlServer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(configure => configure.AddConsole());

        // Select a DB provider. Ensure you reference the appropriate library if necessary.
        services.AddLightNapSqlServer(context.Configuration);

        services.AddTransient<IMaintenanceTask, CountUsersMaintenanceTask>();
        services.AddTransient<IMaintenanceTask, PurgeExpiredRefreshTokensMaintenanceTask>();

        services.AddTransient<MainService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var mainService = services.GetRequiredService<MainService>();
    await mainService.RunAsync();
}