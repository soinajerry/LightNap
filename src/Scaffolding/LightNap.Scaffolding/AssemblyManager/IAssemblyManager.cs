namespace LightNap.Scaffolding.ProjectManager
{
    public interface IAssemblyManager
    {
        Type? LoadType(string assemblyPath, string className);
    }
}
