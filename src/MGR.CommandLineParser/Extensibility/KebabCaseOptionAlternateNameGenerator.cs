using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    ///     Generates alternate names by camel-casing the original name.
    /// </summary>
    public sealed class KebabCaseOptionAlternateNameGenerator : IOptionAlternateNameGenerator
    {
        /// <inheritdoc />
        public IEnumerable<string> GenerateAlternateNames(IOptionDisplayInfo optionDisplayInfo)
        {
            var nameAsKebabCase = optionDisplayInfo.Name.AsKebabCase();
            yield return nameAsKebabCase;
        }
    }
}