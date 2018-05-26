using System;
using System.Collections.Generic;
using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Generates alternate names by camel-casing the original name.
    /// </summary>
    public sealed class KebabCaseOptionAlternateNameGenerator : IOptionAlternateNameGenerator
    {
        /// <inheritdoc cref="GenerateAlternateNames" />
        /// >
        public IEnumerable<string> GenerateAlternateNames(OptionDisplayInfo optionDisplayInfo, PropertyInfo propertyInfo)
        {
            var nameAsKebabCase = optionDisplayInfo.Name.AsKebabCase();
            yield return nameAsKebabCase;
        }
    }
}