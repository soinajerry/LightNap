using Humanizer;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.Helpers
{
    internal class TemplateParameters
    {
        public required string PascalName { get; set; }
        public required string PascalNamePlural { get; set; }
        public required string CamelName { get; set; }
        public required string CamelNamePlural { get; set; }
        public required string KebabName { get; set; }
        public required string KebabNamePlural { get; set; }
        public required string ServerIdType { get; set; }
        public required string ServerPropertiesList { get; set; }
        public required string ServerOptionalPropertiesList { get; set; }
        public required string ServerPropertiesToDto { get; set; }
        public required string ServerPropertiesFromDto { get; set; }
        public required string ClientIdType { get; set; }
        public required string ClientPropertiesList { get; set; }
        public required string ClientOptionalPropertiesList { get; set; }

        [SetsRequiredMembers]
        public TemplateParameters(string pascalName, List<PropertyDetails> propertiesDetails)
        {
            this.PascalName = pascalName;
            this.PascalNamePlural = pascalName.Pluralize();
            this.CamelName = pascalName.Camelize();
            this.CamelNamePlural = this.CamelName.Pluralize();
            this.KebabName = pascalName.Kebaberize();
            this.KebabNamePlural = this.KebabName.Pluralize();

            // Take a guess that the shortest property ending with "id" is the id property.
            PropertyDetails? idProperty = propertiesDetails.Where(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase)).OrderBy(id => id.Name.Length).FirstOrDefault();
            if (idProperty != null)
            {
                this.ServerIdType = this.GetServerTypeString(idProperty.Type);
                this.ClientIdType = this.GetClientTypeString(idProperty.Type);
            }
            else
            {
                this.ServerIdType = "string";
                this.ClientIdType = "string";
            }

            this.ServerPropertiesList = string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {this.GetServerTypeString(p.Type)} {p.Name} {{ get; set; }}"));
            this.ServerOptionalPropertiesList = string.Join("\n\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"public {this.GetServerTypeString(p.Type)}? {p.Name} {{ get; set; }}"));
            this.ServerPropertiesToDto = string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"dto.{p.Name} = item.{p.Name};"));
            this.ServerPropertiesFromDto = string.Join("\n\t\t\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"item.{p.Name} = dto.{p.Name};"));
            this.ClientPropertiesList = string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.Name.Camelize()}: {this.GetClientTypeString(p.Type)};"));
            this.ClientOptionalPropertiesList = string.Join("\n\t", propertiesDetails.Where(p => p != idProperty).Select(p => $"{p.Name.Camelize()}?: {this.GetClientTypeString(p.Type)};"));
        }

        private string GetServerTypeString(Type type)
        {
            // This can be done with compiler services, but it's a lot to add for a simple task.
            if (type == typeof(int)) { return "int"; }
            if (type == typeof(long)) { return "long"; }
            if (type == typeof(string)) { return "string"; }
            if (type == typeof(Guid)) { return "Guid"; }
            if (type == typeof(double)) { return "double"; }
            if (type == typeof(DateTime)) { return "DateTime"; }
            return type.Name;
        }

        private string GetClientTypeString(Type type)
        {
            if (type == typeof(int) || type == typeof(long)) { return "number"; }
            return "string";
        }
    }
}
