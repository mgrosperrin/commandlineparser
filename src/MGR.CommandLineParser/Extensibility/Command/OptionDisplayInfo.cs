using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace MGR.CommandLineParser.Extensibility.Command
{
    /// <summary>
    ///     Represents the display information of an option.
    /// </summary>
    public sealed class OptionDisplayInfo
    {
        /// <summary>
        ///     Creates a new <see cref="OptionDisplayInfo" />.
        /// </summary>
        public OptionDisplayInfo(PropertyInfo propertyInfo)
        {
            Guard.NotNull(propertyInfo, nameof(propertyInfo));
            Name = propertyInfo.Name;
            ShortName = propertyInfo.Name;
            Description = "";
            var displayAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttribute != null)
            {
                Name = displayAttribute.GetName() ?? propertyInfo.Name;
                ShortName = displayAttribute.GetShortName();
                Description = displayAttribute.GetDescription();
            }

            var alternateNames = new List<string>();
            var nameAsKebabCase = Name.AsKebabCase();
            if (!nameAsKebabCase.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
            {
                alternateNames.Add(nameAsKebabCase);
            }
            AlternateNames = alternateNames.AsEnumerable();
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