namespace LightNap.Scaffolding.TemplateManager
{
    public class TemplateManager : ITemplateManager
    {
        public void ProcessTemplate(TemplateItem template, TemplateParameters templateParameters)
        {
            string templateContent = File.ReadAllText(template.TemplateFile);
            string content = TemplateProcessor.ProcessTemplate(templateContent, templateParameters);
            Directory.CreateDirectory(Path.GetDirectoryName(template.OutputFile)!);
            File.WriteAllText(template.OutputFile, content);
        }
    }
}
