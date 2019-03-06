using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    internal sealed class ClassBasedOptionDisplayInfo : IOptionDisplayInfo
    {
        internal ClassBasedOptionDisplayInfo(PropertyInfo propertyInfo, IEnumerable<IPropertyOptionAlternateNameGenerator> optionAlternateNameGenerators)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            Name = propertyInfo.Name;
            ShortName = propertyInfo.Name;
            Description = string.Empty;
            var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttribute != null)
            {
                Name = displayAttribute.GetName() ?? propertyInfo.Name;
                ShortName = displayAttribute.GetShortName();
                Description = displayAttribute.GetDescription();
            }

            AlternateNames = optionAlternateNameGenerators.SelectMany(
                    generator => generator.GenerateAlternateNames(propertyInfo))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Where(alternateName => !alternateName.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public IEnumerable<string> AlternateNames { get; }

        /// <inheritdoc />
        public string ShortName { get; }

        /// <inheritdoc />
        public string Description { get; }
    }
}