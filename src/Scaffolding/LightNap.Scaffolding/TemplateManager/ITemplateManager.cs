namespace LightNap.Scaffolding.TemplateManager
{
    /// <summary>
    /// Interface for managing templates.
    /// </summary>
    public interface ITemplateManager
    {
        /// <summary>
        /// Processes the specified template with the given parameters.
        /// </summary>
        /// <param name="templateItem">The template item to process.</param>
        /// <param name="templateParameters">The parameters to use for processing the template.</param>
        void ProcessTemplate(TemplateItem templateItem, TemplateParameters templateParameters);
    }
}
