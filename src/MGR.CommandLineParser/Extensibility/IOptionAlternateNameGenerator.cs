using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.ClassBased;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// Defines the contract for the generation of alternate names.
    /// </summary>
    public interface IOptionAlternateNameGenerator
    {
        /// <summary>
        ///     Gets the alternate names for an option.
        /// </summary>
        /// <param name="optionDisplayInfo">The <see cref="ClassBasedOptionDisplayInfo" /> for the option.</param>
        /// <param name="propertyInfo">The property info representing the option of the commands.</param>
        /// <returns>The alternate names for the option.</returns>
        IEnumerable<string> GenerateAlternateNames(ClassBasedOptionDisplayInfo optionDisplayInfo, PropertyInfo propertyInfo);
    }
}