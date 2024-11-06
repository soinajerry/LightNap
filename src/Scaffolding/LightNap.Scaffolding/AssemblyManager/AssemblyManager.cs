using System.Reflection;

namespace LightNap.Scaffolding.ProjectManager
{
    public class AssemblyManager : IAssemblyManager
    {
        public Type? LoadType(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(className);
        }
    }
}
