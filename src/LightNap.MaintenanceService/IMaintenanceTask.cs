namespace LightNap.MaintenanceService
{
    internal interface IMaintenanceTask
    {
        string Name { get; }
        Task RunAsync();
    }
}
