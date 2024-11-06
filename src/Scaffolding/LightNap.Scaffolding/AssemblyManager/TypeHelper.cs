namespace LightNap.Scaffolding.AssemblyManager
{
    public static class TypeHelper
    {
        public static List<TypePropertyDetails> GetPropertyDetails(Type type)
        {
            List<TypePropertyDetails> propertiesDetails = [];

            foreach (var property in type.GetProperties())
            {
                try
                {
                    // Check if the property type is a common Entity Framework type
                    if (property.PropertyType.IsPrimitive ||
                        property.PropertyType == typeof(string) ||
                        property.PropertyType == typeof(DateTime) ||
                        property.PropertyType == typeof(Guid) ||
                        property.PropertyType == typeof(decimal) ||
                        property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                            (Nullable.GetUnderlyingType(property.PropertyType) == typeof(int) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(DateTime) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(Guid) ||
                            Nullable.GetUnderlyingType(property.PropertyType) == typeof(decimal)))
                    {
                        propertiesDetails.Add(new TypePropertyDetails(property.PropertyType, property.Name));
                        Console.WriteLine($"Found {property.Name} ({property.PropertyType.Name})");
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring '{property.Name}': Not a type supported in this scaffolder  ({property.PropertyType.Name})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ignoring '{property.Name}': {ex.Message}");
                }
            }

            return propertiesDetails;
        }
    }
}
