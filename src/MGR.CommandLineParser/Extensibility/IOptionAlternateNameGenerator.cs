using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

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
        /// <param name="optionDisplayInfo">The <see cref="IOptionDisplayInfo" /> for the option.</param>
        /// <returns>The alternate names for the option.</returns>
        IEnumerable<string> GenerateAlternateNames(IOptionDisplayInfo optionDisplayInfo);
    }
}