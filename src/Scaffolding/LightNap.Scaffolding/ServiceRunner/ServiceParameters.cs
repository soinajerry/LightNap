using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.ServiceRunner
{
    /// <summary>
    /// Represents the parameters required for the service.
    /// </summary>
    public class ServiceParameters
    {
        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public required string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public required string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the core project name.
        /// </summary>
        public required string CoreProjectName { get; set; }

        /// <summary>
        /// Gets or sets the Web API project name.
        /// </summary>
        public required string WebApiProjectName { get; set; }

        /// <summary>
        /// Gets or sets the Angular project name.
        /// </summary>
        public required string AngularProjectName { get; set; }

        /// <summary>
        /// Gets or sets the Web API project path.
        /// </summary>
        public required string WebApiProjectPath { get; set; }

        /// <summary>
        /// Gets or sets the Web API project file path.
        /// </summary>
        public required string WebApiProjectFilePath { get; set; }

        /// <summary>
        /// Gets or sets the core project path.
        /// </summary>
        public required string CoreProjectPath { get; set; }

        /// <summary>
        /// Gets or sets the core project file path.
        /// </summary>
        public required string CoreProjectFilePath { get; set; }

        /// <summary>
        /// Gets or sets the client application path.
        /// </summary>
        public required string ClientAppPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceParameters"/> class.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="coreProjectName">The core project name.</param>
        /// <param name="webApiProjectName">The Web API project name.</param>
        /// <param name="angularProjectName">The Angular project name.</param>
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
