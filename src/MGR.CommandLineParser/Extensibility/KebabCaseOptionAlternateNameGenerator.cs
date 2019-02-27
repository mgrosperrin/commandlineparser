using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.ClassBased;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    ///     Generates alternate names by camel-casing the original name.
    /// </summary>
    public sealed class KebabCaseOptionAlternateNameGenerator : IOptionAlternateNameGenerator
    {
        /// <inheritdoc cref="GenerateAlternateNames" />
        /// >
        public IEnumerable<string> GenerateAlternateNames(ClassBasedOptionDisplayInfo optionDisplayInfo, PropertyInfo propertyInfo)
        {
            var nameAsKebabCase = optionDisplayInfo.Name.AsKebabCase();
            yield return nameAsKebabCase;
        }
    }
}