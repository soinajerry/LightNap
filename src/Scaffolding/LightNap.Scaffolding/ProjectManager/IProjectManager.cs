namespace LightNap.Scaffolding.ProjectManager
{
    public interface IProjectManager
    {
        ProjectBuildResult BuildProject(string projectPath);
        void AddFileToProject(string projectPath, string filePath);
    }
}
