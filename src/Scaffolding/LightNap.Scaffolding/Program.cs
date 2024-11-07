using LightNap.Scaffolding.AssemblyManager;
using LightNap.Scaffolding.ProjectManager;
using LightNap.Scaffolding.ServiceRunner;
using LightNap.Scaffolding.TemplateManager;
using System.CommandLine;
using System.CommandLine.Parsing;

Argument<string> classNameArgument =
    new("className",
        description: "The name of the entity to scaffold for");
Option<string> namespaceOption =
    new("--namespace",
        getDefaultValue: () => "LightNap.Core.Data.Entities",
        description: "The namespace of the entity");
Option<string> srcPathOption =
    new("--src-path",
        getDefaultValue: () => "./",
        description: "The path to the /src folder of the repo");
Option<string> coreProjectNameOption =
    new("--core-project",
        getDefaultValue: () => "LightNap.Core",
        description: "The name of the core project");
Option<string> webApiProjectNameOption =
    new("--web-api-project",
        getDefaultValue: () => "LightNap.WebApi",
        description: "The name of the web API project");
Option<string> angularProjectNameOption =
    new("--angular-project",
        getDefaultValue: () => "lightnap-ng",
        description: "The name of the Angular project");

var rootCommand = new RootCommand()
{
    classNameArgument,
    namespaceOption,
    srcPathOption,
    coreProjectNameOption,
    webApiProjectNameOption,
    angularProjectNameOption,
};

rootCommand.SetHandler((className, namespaceValue, srcPath, coreProjectName, webApiProjectName, angularProjectName) =>
{
    ServiceRunner runner = new(new ProjectManager(), new TemplateManager(), new AssemblyManager());
    runner.Run(new ServiceParameters($"{namespaceValue}.{className}", srcPath, coreProjectName, webApiProjectName, angularProjectName));
},
classNameArgument, namespaceOption, srcPathOption, coreProjectNameOption, webApiProjectNameOption, angularProjectNameOption);

return await rootCommand.InvokeAsync(args);