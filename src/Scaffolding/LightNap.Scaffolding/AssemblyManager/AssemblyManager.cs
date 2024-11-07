using System.Reflection;

namespace LightNap.Scaffolding.ProjectManager
{
    public class AssemblyManager : IAssemblyManager
    {
        public string GetExecutingPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        }

        public Type? LoadType(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(className);
        }
    }
}
