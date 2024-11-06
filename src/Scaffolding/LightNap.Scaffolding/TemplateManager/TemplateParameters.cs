using Humanizer;
using LightNap.Scaffolding.AssemblyManager;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.TemplateManager
{
    public class TemplateParameters
    {
        private Dictionary<string, string> _replacements = [];
        public IReadOnlyDictionary<string, string> Replacements => this._replacements.AsReadOnly();

        [SetsRequiredMembers]
        public TemplateParameters(string pascalName, List<TypePropertyDetails> propertiesDetails)
        {
            this._replacements.Add("PascalName", pascalName);
            this._replacements.Add("PascalNamePlural", pascalName.Pluralize());

            // If the pluralized name is the same as the singular name, add an underscore to the name so that we don't get ambiguity errors in the generated code due to
            // the namespace and type being identical. This seemed like the least impactful way to fix the issue.
            this._replacements.Add("NameForNamespace", pascalName.Pluralize());
            if (this.Replacements["PascalName"] == this.Replacements["NameForNamespace"]) { this._replacements["NameForNamespace"] = $"{pascalName}_"; }

            this._replacements.Add("CamelName", pascalName.Camelize());
            this._replacements.Add("CamelNamePlural", pascalName.Camelize().Pluralize());
            this._replacements.Add("KebabName", pascalName.Kebaberize());
            this._replacements.Add("KebabNamePlural", pascalName.Kebaberize().Pluralize());

            // Take a guess that the shortest property ending with "id" is the id property.
            TypePropertyDetails? idProperty = propertiesDetails.Where(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase)).OrderBy(id => id.Name.Length).FirstOrDefault();
            this._replacements.Add("ClientIdType", idProperty?.ClientTypeString ?? "string");
            this._replacements.Add("ServerIdType", idProperty?.ServerTypeString ?? "string");

            // This was the point where I realized I would someday need to use a real template processor. But today would not be that day.

            this._replacements.Add("ServerPropertiesList", string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {p.ServerTypeString} {p.Name} {{ get; set; }}")));
            this._replacements.Add("ServerOptionalPropertiesList", string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {p.ServerTypeString}? {p.Name} {{ get; set; }}")));
            this._replacements.Add("ServerPropertiesToDto", string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"dto.{p.Name} = item.{p.Name};")));
            this._replacements.Add("ServerPropertiesFromDto", string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"item.{p.Name} = dto.{p.Name};")));
            this._replacements.Add("ClientPropertiesList", string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.CamelName}: {p.ClientTypeString};")));
            this._replacements.Add("ClientOptionalPropertiesList", string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.CamelName}?: {p.ClientTypeString};")));
        }
    }
}
