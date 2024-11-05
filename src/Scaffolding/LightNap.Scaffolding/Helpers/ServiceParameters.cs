using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.Helpers
{
    internal class ServiceParameters
    {
        public required string ClassName { get; set; }
        public required string SourcePath { get; set; }
        public required string CoreProjectName { get; set; }
        public required string WebApiProjectName { get; set; }
        public required string AngularProjectName { get; set; }
        public required string WebApiProjectPath { get; set; }
        public required string WebApiProjectFilePath { get; set; }
        public required string CoreProjectPath { get; set; }
        public required string CoreProjectFilePath { get; set; }
        public required string ClientAppPath { get; set; }

        [SetsRequiredMembers]
        public ServiceParameters(string className, string sourcePath, string coreProjectName, string webApiProjectName, string angularProjectName)
        {
            this.ClassName = className;
            this.SourcePath = Path.GetFullPath(sourcePath);
            this.CoreProjectName = coreProjectName;
            this.WebApiProjectName = webApiProjectName;
            this.AngularProjectName = angularProjectName;

            this.WebApiProjectPath = Path.Combine(this.SourcePath, webApiProjectName);
            this.WebApiProjectFilePath = Path.Combine(this.WebApiProjectPath, $"{webApiProjectName}.csproj");
            this.CoreProjectPath = Path.Combine(this.SourcePath, coreProjectName);
            this.CoreProjectFilePath = Path.Combine(this.CoreProjectPath, $"{coreProjectName}.csproj");
            this.ClientAppPath = Path.Combine(this.SourcePath, angularProjectName, "src/app");
        }
    }
}
