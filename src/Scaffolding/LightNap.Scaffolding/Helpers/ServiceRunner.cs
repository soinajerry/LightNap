using Microsoft.Build.Locator;
using System.Reflection;

namespace LightNap.Scaffolding.Helpers
{
    internal class ServiceRunner
    {
        public void Run(ServiceParameters parameters)
        {
            if (!ValidateParameters(parameters))
            {
                return;
            }

            MSBuildLocator.RegisterDefaults();

            string? assemblyPath = ProjectHelper.BuildProject(parameters.CoreProjectFilePath);
            if (assemblyPath is null)
            {
                Console.WriteLine("Please fix the build failures and try again.");
                return;
            }

            var assembly = Assembly.LoadFrom(assemblyPath);

            // Reflect against the assembly
            var type = assembly.GetType(parameters.ClassName);
            if (type == null)
            {
                Console.WriteLine($"Type '{parameters.ClassName}' not found or could not be loaded in assembly.");
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
                new("Server/CreateDto.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Dto/Request/Create{type.Name}Dto.cs", parameters.CoreProjectFilePath),
                new("Server/Dto.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Dto/Response/{type.Name}Dto.cs", parameters.CoreProjectFilePath),
                new("Server/Extensions.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Extensions/{type.Name}Extensions.cs", parameters.CoreProjectFilePath),
                new("Server/Interface.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Interfaces/I{type.Name}Service.cs", parameters.CoreProjectFilePath),
                new("Server/SearchDto.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Dto/Request/Search{type.Name}Dto.cs", parameters.CoreProjectFilePath),
                new("Server/Service.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Services/{type.Name}Service.cs", parameters.CoreProjectFilePath),
                new("Server/UpdateDto.cs.txt", $"{parameters.CoreProjectPath}/{templateParameters.PascalNamePlural}/Dto/Request/Update{type.Name}Dto.cs", parameters.CoreProjectFilePath),
                new("Server/Controller.cs.txt", $"{parameters.WebApiProjectPath}/Controllers/{templateParameters.PascalName}Controller.cs", parameters.WebApiProjectFilePath),

                new("Client/routes.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/routes.ts"),
                new("Client/index.component.html.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/index/index.component.html"),
                new("Client/index.component.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/index/index.component.ts"),
                new("Client/get.component.html.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/get/get.component.html"),
                new("Client/get.component.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/get/get.component.ts"),
                new("Client/create.component.html.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/create/create.component.html"),
                new("Client/create.component.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/create/create.component.ts"),
                new("Client/edit.component.html.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/edit/edit.component.html"),
                new("Client/edit.component.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/components/pages/edit/edit.component.ts"),
                new("Client/create-request.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/models/request/create-{templateParameters.KebabName}-request.ts"),
                new("Client/search-request.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/models/request/search-{templateParameters.KebabNamePlural}-request.ts"),
                new("Client/update-request.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/models/request/update-{templateParameters.KebabName}-request.ts"),
                new("Client/response.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/models/response/{templateParameters.KebabName}.ts"),
                new("Client/data.service.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/services/data.service.ts"),
                new("Client/service.ts.txt", $"{parameters.ClientAppPath}/{templateParameters.KebabNamePlural}/services/{templateParameters.KebabName}.service.ts"),
            };

            foreach (var template in templateItems)
            {
                if (File.Exists(Path.Combine(parameters.SourcePath, template.OutputFile)))
                {
                    Console.WriteLine($"Bailing out: File '{template.OutputFile}' already exists!");
                    return;
                }
            }

            foreach (var template in templateItems)
            {
                string templatePath = Path.Combine("Templates", template.TemplateFile);

                string content = TemplateProcessor.ProcessTemplate(templatePath, templateParameters);
                Directory.CreateDirectory(Path.GetDirectoryName(template.OutputFile)!);
                TemplateProcessor.WriteToFile(template.OutputFile, content);

                if (template.ProjectPath != null)
                {
                    ProjectHelper.AddFileToProject(template.ProjectPath, template.OutputFile);
                }

                Console.WriteLine($"Generated {template.OutputFile}");
            }

            Console.WriteLine(
@$"Scaffolding completed successfully. Please see TODO comments in generated code to complete integration.

{parameters.CoreProjectName}:
- Update client and server DTO properties in {templateParameters.PascalNamePlural}/Dto to only those you want included.
- Update extension method mappers between DTOs and the entity in Extensions/{type.Name}Extensions.cs.

{parameters.WebApiProjectName}:
- Update the authorization for methods in Controllers/{templateParameters.PascalNamePlural}Controller.cs based on access preferences.
- Register Web API controller parameter dependency in Extensions/ApplicationServiceExtensions.cs.

{parameters.AngularProjectName}:
- Update the models in {templateParameters.KebabNamePlural}/models to match the updated server DTOs.
- Update authorization for the routes in {templateParameters.KebabNamePlural}/components/pages/routes.ts.
- Add {templateParameters.KebabNamePlural} routes to the root route collection in routing/routes.ts.");
        }

        public static bool ValidateParameters(ServiceParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.SourcePath))
            {
                Console.WriteLine("Path to /src is required.");
                return false;
            }

            if (!File.Exists(parameters.WebApiProjectFilePath))
            {
                Console.WriteLine($"Web API project not found at: {parameters.WebApiProjectFilePath}");
                return false;
            }

            if (!File.Exists(parameters.CoreProjectFilePath))
            {
                Console.WriteLine($"Core project not found at: {parameters.CoreProjectFilePath}");
                return false;
            }

            if (!Directory.Exists(parameters.ClientAppPath))
            {
                Console.WriteLine($"Angular project not found at: {parameters.ClientAppPath}");
                return false;
            }

            return true;
        }
    }
}