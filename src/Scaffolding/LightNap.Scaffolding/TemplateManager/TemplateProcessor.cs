namespace LightNap.Scaffolding.TemplateManager
{
    /// <summary>
    /// Provides methods to process templates with specified parameters.
    /// </summary>
    public static class TemplateProcessor
    {
        /// <summary>
        /// Processes the template content by replacing placeholders with specified values.
        /// </summary>
        /// <param name="templateContent">The content of the template to process.</param>
        /// <param name="templateParameters">The parameters containing the replacements.</param>
        /// <returns>The processed template content.</returns>
        public static string ProcessTemplate(string templateContent, TemplateParameters templateParameters)
        {
            foreach (var replacement in templateParameters.Replacements)
            {
                templateContent = templateContent.Replace($"<#= {replacement.Key} #>", replacement.Value);
            }
            return templateContent;
        }
    }
}