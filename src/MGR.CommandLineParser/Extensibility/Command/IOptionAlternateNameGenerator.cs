using System.Collections.Generic;
using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    /// Defines the contract for the generation of altername names.
    /// </summary>
    public interface IOptionAlternateNameGenerator
    {
        /// <summary>
        ///     Gets the alternate names for an option.
        /// </summary>
        /// <param name="optionDisplayInfo">The <see cref="OptionDisplayInfo" /> for the option.</param>
        /// <param name="propertyInfo">The property info representing the option of the commands.</param>
        /// <returns>The alternate names for the option.</returns>
        IEnumerable<string> GenerateAlternateNames(OptionDisplayInfo optionDisplayInfo, PropertyInfo propertyInfo);
    }
}