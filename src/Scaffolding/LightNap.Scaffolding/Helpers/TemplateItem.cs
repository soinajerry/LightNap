using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.Helpers
{
    [method: SetsRequiredMembers]
    internal class TemplateItem(string templateFile, string outputFile, string? projectPath = null)
    {
        public required string TemplateFile { get; set; } = templateFile;
        public required string OutputFile { get; set; } = outputFile;
        public string? ProjectPath { get; set; } = projectPath;
    }
}
