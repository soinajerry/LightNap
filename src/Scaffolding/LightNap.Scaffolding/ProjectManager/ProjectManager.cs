using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Locator;
using Microsoft.Build.Logging;

namespace LightNap.Scaffolding.ProjectManager
{
    public class ProjectManager : IProjectManager
    {
        public ProjectManager()
        {
            MSBuildLocator.RegisterDefaults();
        }

        public ProjectBuildResult BuildProject(string projectPath)
        {
            Console.WriteLine($"Attempting to build project at: {projectPath}");

            var projectCollection = new ProjectCollection();
            var project = projectCollection.LoadProject(projectPath);

            var buildParameters = new BuildParameters(projectCollection)
            {
                Loggers = [new ConsoleLogger(LoggerVerbosity.Quiet)]
            };

            var buildRequest = new BuildRequestData(project.CreateProjectInstance(), ["Build"]);
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            if (buildResult.OverallResult != BuildResultCode.Success)
            {
                foreach (var item in buildResult.ResultsByTarget)
                {
                    Console.WriteLine($"{item.Key}: {item.Value.ResultCode}");
                }
                return new ProjectBuildResult() { Success = false };
            }

            var outputPath = project.GetPropertyValue("OutputPath");
            var outputFileName = project.GetPropertyValue("AssemblyName") + ".dll";
            return new ProjectBuildResult()
            {
                OutputAssemblyPath = Path.Combine(project.DirectoryPath, outputPath, outputFileName),
                Success = true
            };
        }

        public void AddFileToProject(string projectPath, string filePath)
        {
            var projectCollection = new ProjectCollection();
            var project = projectCollection.LoadProject(projectPath);

            var relativeFilePath = Path.GetRelativePath(Path.GetDirectoryName(projectPath)!, filePath);
            project.AddItem("Compile", relativeFilePath);

            project.Save();
        }
    }
}
