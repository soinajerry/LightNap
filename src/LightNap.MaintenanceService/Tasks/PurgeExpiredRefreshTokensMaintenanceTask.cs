
using LightNap.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LightNap.MaintenanceService.Tasks
{
    internal class PurgeExpiredRefreshTokensMaintenanceTask(ILogger<PurgeExpiredRefreshTokensMaintenanceTask> logger, ApplicationDbContext db) : IMaintenanceTask
    {
        public string Name => "Purge Expired Refresh Tokens";

        public async Task RunAsync()
        {
            logger.LogInformation("Starting with {count} refresh tokens", await db.RefreshTokens.CountAsync());

            const int batchSize = 100;

            int deletedCount = 0;

            while (true)
            {
                var expiredTokens = await db.RefreshTokens.Where(token => token.Expires < DateTime.UtcNow).OrderByDescending(token => token.Id).Take(batchSize).ToListAsync();
                if (expiredTokens.Count == 0) { break; }

                db.RefreshTokens.RemoveRange(expiredTokens);
                deletedCount += await db.SaveChangesAsync();
            }

            logger.LogInformation("Deleted {deletedCount} expired refresh tokens", deletedCount);

            // It's possible that some may have been created since we started.
            logger.LogInformation("Finished with {count} refresh tokens", await db.RefreshTokens.CountAsync());
        }
    }
}
