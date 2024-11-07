namespace LightNap.Scaffolding.ProjectManager
{
    public interface IAssemblyManager
    {
        string GetExecutingPath();
        Type? LoadType(string assemblyPath, string className);
    }
}
