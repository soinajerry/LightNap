using System.Text;

namespace LightNap.Scaffolding.Helpers
{
    internal static class TemplateProcessor
    {
        public static string ProcessTemplate(string templatePath, TemplateParameters templateParameters)
        {
            // Not an ideal solution, but straightforward and simple.
            string templateContent = File.ReadAllText(templatePath);
            templateContent = templateContent
                .Replace("<#= CamelName #>", templateParameters.CamelName)
                .Replace("<#= CamelNamePlural #>", templateParameters.CamelNamePlural)
                .Replace("<#= KebabName #>", templateParameters.KebabName)
                .Replace("<#= KebabNamePlural #>", templateParameters.KebabNamePlural)
                .Replace("<#= PascalName #>", templateParameters.PascalName)
                .Replace("<#= PascalNamePlural #>", templateParameters.PascalNamePlural)
                .Replace("<#= ClientPropertiesList #>", templateParameters.ClientPropertiesList)
                .Replace("<#= ClientOptionalPropertiesList #>", templateParameters.ClientOptionalPropertiesList)
                .Replace("<#= ServerPropertiesList #>", templateParameters.ServerPropertiesList)
                .Replace("<#= ServerOptionalPropertiesList #>", templateParameters.ServerOptionalPropertiesList)
                .Replace("<#= ServerPropertiesFromDto #>", templateParameters.ServerPropertiesFromDto)
                .Replace("<#= ServerPropertiesToDto #>", templateParameters.ServerPropertiesToDto)
                .Replace("<#= ClientIdType #>", templateParameters.ClientIdType)
                .Replace("<#= ServerIdType #>", templateParameters.ServerIdType);
            return templateContent;
        }

        public static void WriteToFile(string outputPath, string content)
        {
            File.WriteAllText(outputPath, content, Encoding.UTF8);
        }
    }
}