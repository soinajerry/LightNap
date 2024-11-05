using LightNap.Scaffolding.Helpers;
using Microsoft.Build.Locator;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;

Argument<string> classNameArgument =
    new(
        "className",
        description: "The name of the entity to scaffold for");
Option<string> srcPathOption =
    new(
        "--src-path",
        getDefaultValue: () => "./",
        description: "The path to the /src folder of the repo");

var rootCommand = new RootCommand()
{
    classNameArgument,
    srcPathOption,
};

rootCommand.SetHandler((className, srcPath) =>
{
    string coreProjectName = "LightNap.Core";
    string webApiProjectName = "LightNap.WebApi";
    string angularProjectName = "lightnap-ng";

    if (string.IsNullOrEmpty(srcPath))
    {
        Console.WriteLine("Path to /src is required.");
        return;
    }

    string webApiProjectPath = Path.Combine(srcPath, webApiProjectName, $"{webApiProjectName}.csproj");
    string coreProjectPath = Path.Combine(srcPath, coreProjectName, $"{coreProjectName}.csproj");
    string clientAppPath = Path.Combine(srcPath, angularProjectName, "src/app");
    Console.WriteLine($"Building project at: {coreProjectPath}");
    MSBuildLocator.RegisterDefaults();
    if (!ProjectHelper.BuildProject(coreProjectPath))
    {
        Console.WriteLine("Please fix the build failures and try again.");
        return;
    }

    // Load the built assembly
    var assemblyPath = Path.Combine(srcPath, coreProjectName, $"bin/Debug/net8.0/{coreProjectName}.dll");
    var assembly = Assembly.LoadFrom(assemblyPath);

    // Reflect against the assembly
    var type = assembly.GetType(className);
    if (type == null)
    {
        Console.WriteLine($"Type '{className}' not found or could not be loaded in assembly.");
        return;
    }

    Console.WriteLine($"Type name: {type.Name} ({type.FullName})");

    // Your reflection and scaffolding logic here

    // List all public properties
    var properties = type.GetProperties();
    Console.WriteLine($"Public properties of {type.Name}:");

    List<PropertyDetails> propertiesDetails = [];

    foreach (var property in properties)
    {
        try
        {
            // Check if the property type is a common Entity Framework type
            if (property.PropertyType.IsPrimitive ||
                property.PropertyType == typeof(string) ||
                property.PropertyType == typeof(DateTime) ||
                property.PropertyType == typeof(Guid) ||
                property.PropertyType == typeof(decimal) ||
                (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                    (Nullable.GetUnderlyingType(property.PropertyType) == typeof(int) ||
                    Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime) ||
                    Nullable.GetUnderlyingType(property.PropertyType) == typeof(Guid) ||
                    Nullable.GetUnderlyingType(property.PropertyType) == typeof(decimal))))
            {
                propertiesDetails.Add(new PropertyDetails(property.PropertyType, property.Name));
                Console.WriteLine($"Found {property.Name} ({property.PropertyType.Name})");
            }
            else
            {
                Console.WriteLine($"Ignoring '{property.Name}': Not a type supported in this scaffolder  ({property.PropertyType.Name})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ignoring '{property.Name}': {ex.Message}");
        }
    }

    TemplateParameters templateParameters = new(type.Name, propertiesDetails);

    var templateItems = new List<TemplateItem>
    {
        new("Server/CreateDto.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Dto/Request/Create{type.Name}Dto.cs", coreProjectPath),
        new("Server/Dto.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Dto/Response/{type.Name}Dto.cs", coreProjectPath),
        new("Server/Extensions.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Extensions/{type.Name}Extensions.cs", coreProjectPath),
        new("Server/Interface.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Interfaces/I{type.Name}Service.cs", coreProjectPath),
        new("Server/SearchDto.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Dto/Request/Search{type.Name}Dto.cs", coreProjectPath),
        new("Server/Service.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Services/{type.Name}Service.cs", coreProjectPath),
        new("Server/UpdateDto.cs.txt", $"{coreProjectName}/{templateParameters.PascalNamePlural}/Dto/Request/Update{type.Name}Dto.cs", coreProjectPath),
        new("Server/Controller.cs.txt", $"{webApiProjectName}/Controllers/{templateParameters.PascalNamePlural}Controller.cs", webApiProjectPath),

        new("Client/routes.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/components/pages/routes.ts"),
        new("Client/index.component.html.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/components/pages/index/index.component.html"),
        new("Client/index.component.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/components/pages/index/index.component.ts"),
        new("Client/get.component.html.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/components/pages/get/get.component.html"),
        new("Client/get.component.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/components/pages/get/get.component.ts"),
        new("Client/create-request.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/models/request/create-{templateParameters.KebabName}-request.ts"),
        new("Client/search-request.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/models/request/search-{templateParameters.KebabNamePlural}-request.ts"),
        new("Client/update-request.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/models/request/update-{templateParameters.KebabName}-request.ts"),
        new("Client/response.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/models/response/{templateParameters.KebabName}.ts"),
        new("Client/data.service.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/services/data.service.ts"),
        new("Client/service.ts.txt", $"{clientAppPath}/{templateParameters.KebabNamePlural}/services/{templateParameters.KebabName}.service.ts"),
    };

    foreach (var template in templateItems)
    {
        if (File.Exists(Path.Combine(srcPath, template.OutputFile)))
        {
            Console.WriteLine($"Bailing out: File '{template.OutputFile}' already exists!");
            return;
        }
    }

    foreach (var template in templateItems)
    {
        string templatePath = Path.Combine("Templates", template.TemplateFile);
        string outputPath = Path.Combine(srcPath, template.OutputFile);

        string content = TemplateProcessor.ProcessTemplate(templatePath, templateParameters);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        TemplateProcessor.WriteToFile(outputPath, content);

        if (template.ProjectPath != null)
        {
            ProjectHelper.AddFileToProject(template.ProjectPath, outputPath);
        }

        Console.WriteLine($"Generated {outputPath}");
    }


}, classNameArgument, srcPathOption);

// Parse the incoming args and invoke the handler
return rootCommand.InvokeAsync(args).Result;