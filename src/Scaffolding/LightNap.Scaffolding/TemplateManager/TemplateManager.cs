namespace LightNap.Scaffolding.TemplateManager
{
    /// <summary>
    /// Manages the processing of templates.
    /// </summary>
    public class TemplateManager : ITemplateManager
    {
        /// <summary>
        /// Processes the specified template with the given parameters.
        /// </summary>
        /// <param name="template">The template item to process.</param>
        /// <param name="templateParameters">The parameters to use for processing the template.</param>
        public void ProcessTemplate(TemplateItem template, TemplateParameters templateParameters)
        {
            string templateContent = File.ReadAllText(template.TemplateFile);
            string content = TemplateProcessor.ProcessTemplate(templateContent, templateParameters);
            Directory.CreateDirectory(Path.GetDirectoryName(template.OutputFile)!);
            File.WriteAllText(template.OutputFile, content);
        }
    }
}
