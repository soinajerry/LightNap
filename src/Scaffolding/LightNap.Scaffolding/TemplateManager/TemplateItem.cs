using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.TemplateManager
{
    [method: SetsRequiredMembers]
    public class TemplateItem(string templateFile, string outputFile, string? addToProjectPath = null)
    {
        public required string TemplateFile { get; set; } = templateFile;
        public required string OutputFile { get; set; } = outputFile;
        public string? AddToProjectPath { get; set; } = addToProjectPath;
    }
}
