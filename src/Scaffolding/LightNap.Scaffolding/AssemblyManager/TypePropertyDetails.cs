using Humanizer;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.AssemblyManager
{
    public class TypePropertyDetails
    {
        public required Type Type { get; set; }
        public required string Name { get; set; }
        public required string CamelName { get; set; }
        public required string ServerTypeString { get; set; }
        public required string ClientTypeString { get; set; }

        [SetsRequiredMembers]
        public TypePropertyDetails(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
            this.CamelName = this.Name.Camelize();
            this.ServerTypeString = TypePropertyDetails.GetServerTypeString(type);
            this.ClientTypeString = TypePropertyDetails.GetClientTypeString(type);
        }

        public static string GetServerTypeString(Type type)
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

        public static string GetClientTypeString(Type type)
        {
            if (type == typeof(int) || type == typeof(long)) { return "number"; }
            return "string";
        }
    }
}
