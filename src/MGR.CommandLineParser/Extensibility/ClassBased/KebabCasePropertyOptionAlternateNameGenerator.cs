using System;
using System.Collections.Generic;
using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Generates alternate names by camel-casing the original name.
    /// </summary>
    public sealed class KebabCasePropertyOptionAlternateNameGenerator : IPropertyOptionAlternateNameGenerator
    {
        /// <inheritdoc />
        public IEnumerable<string> GenerateAlternateNames(PropertyInfo propertyInfo)
        {
            var nameAsKebabCase = propertyInfo.Name.AsKebabCase();
            yield return nameAsKebabCase;
        }
    }
}