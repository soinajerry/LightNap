using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace LightNap.Scaffolding.Helpers
{
    internal class ProjectHelper
    {
        public static string? BuildProject(string projectPath)
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
                return null;
            }

            var outputPath = project.GetPropertyValue("OutputPath");
            var outputFileName = project.GetPropertyValue("AssemblyName") + ".dll";
            return Path.Combine(project.DirectoryPath, outputPath, outputFileName);
        }

        public static void AddFileToProject(string projectPath, string filePath)
        {
            var projectCollection = new ProjectCollection();
            var project = projectCollection.LoadProject(projectPath);

            project.AddItem("Compile", filePath);

            project.Save();
        }
    }
}
