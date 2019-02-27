using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Represents the display information of an option.
    /// </summary>
    public sealed class ClassBasedOptionDisplayInfo : IOptionDisplayInfo
    {
        /// <summary>
        ///     Creates a new <see cref="ClassBasedOptionDisplayInfo" />.
        /// </summary>
        public ClassBasedOptionDisplayInfo(PropertyInfo propertyInfo, IEnumerable<IOptionAlternateNameGenerator> optionAlternateNameGenerators)
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
                    generator => generator.GenerateAlternateNames(this, propertyInfo))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .Where(alternateName => !alternateName.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        ///     Gets the name of the option.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the alternates names of the option.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public IEnumerable<string> AlternateNames { get; }

        /// <summary>
        ///     Gets the shortname of the option.
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        ///     Gets the description of the option.
        /// </summary>
        public string Description { get; }
    }
}