using System.Diagnostics.CodeAnalysis;

namespace LightNap.Scaffolding.Helpers
{
    [method: SetsRequiredMembers]
    internal class PropertyDetails(Type type, string name)
    {
        public required Type Type { get; set; } = type;
        public required string Name { get; set; } = name;
    }
}
