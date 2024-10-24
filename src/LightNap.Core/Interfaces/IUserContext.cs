namespace LightNap.Core.Interfaces
{
    /// <summary>  
    /// Provides methods to access content specific to a user request.
    /// </summary>  
    public interface IUserContext
    {
        string GetUserId();
        string? GetIpAddress();
    }
}