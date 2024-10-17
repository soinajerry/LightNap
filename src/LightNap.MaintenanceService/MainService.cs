using Microsoft.Extensions.Logging;

namespace LightNap.MaintenanceService
{
    internal class MainService(ILogger<MainService> logger, IEnumerable<IMaintenanceTask> tasks)
    {
        public async Task RunAsync()
        {
            logger.LogInformation($"Starting maintenance run");

            foreach (var task in tasks)
            {
                logger.LogInformation("Starting '{task}'", task.Name);

                try
                {
                    await task.RunAsync();
                    logger.LogInformation("Completed '{task}'", task.Name);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error occurred during '{task}': {e}", task.Name, e);
                }
            }

            logger.LogInformation($"Completed maintenance run");
        }
    }
}
