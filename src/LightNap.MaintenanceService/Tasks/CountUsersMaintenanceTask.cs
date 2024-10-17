
using LightNap.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LightNap.MaintenanceService.Tasks
{
    internal class CountUsersMaintenanceTask(ILogger<CountUsersMaintenanceTask> logger, ApplicationDbContext db) : IMaintenanceTask
    {
        public string Name => "Count Users";

        public async Task RunAsync()
        {
            logger.LogInformation("There are {count} registered users", await db.Users.CountAsync());
        }
    }
}
