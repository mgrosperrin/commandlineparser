using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;

// ReSharper disable CheckNamespace

namespace System.Reflection
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extensions methods for the type <see cref="PropertyInfo"/>.
    /// </summary>
    internal static class PropertyInfoExtensions
    {
        internal static bool IsValidOptionProperty(this PropertyInfo source)
        {
            Guard.NotNull(source, nameof(source));

            return source.CanWrite || source.PropertyType.IsMultiValuedType();
        }

        internal static IConverter ExtractConverter(this PropertyInfo source, List<IConverter> converters, string optionName, string commandName)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNullOrEmpty(optionName, nameof(optionName));
            Guard.NotNullOrEmpty(commandName, nameof(commandName));

            var converter = GetConverterFromAttribute(source, optionName, commandName)
                                ?? GetKeyValueConverterFromAttribute(source, optionName, commandName)
                                ?? FindConverter(source, converters, optionName, commandName);
            if (converter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoConverterFound(optionName, commandName, source.PropertyType));
            }
            return converter;
        }
        private static IConverter FindConverter(PropertyInfo propertyInfo, List<IConverter> converters, string optionName, string commandName)
        {
            if (propertyInfo.PropertyType.IsDictionaryType())
            {
                var keyConverter = FindKeyConverter(propertyInfo, converters, optionName, commandName);
                var valueConverter = FindValueConverter(propertyInfo, converters, optionName, commandName);

                return new KeyValueConverter(keyConverter, valueConverter);
            }
            var converter = GetConverterFromType(propertyInfo.PropertyType, converters);
            return converter;
        }
        private static IConverter FindValueConverter(PropertyInfo propertyInfo, IEnumerable<IConverter> converters, string optionName, string commandName)
        {
            var valueType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(false);
            var valueConverter = GetConverterFromType(valueType, converters);
            if (valueConverter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoValueConverterFound(optionName, commandName, valueType));
            }
            return valueConverter;
        }
        private static IConverter FindKeyConverter(PropertyInfo propertyInfo, IEnumerable<IConverter> converters, string optionName, string commandName)
        {
            var keyType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(true);
            var keyConverter = GetConverterFromType(keyType, converters);
            if (keyConverter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoKeyConverterFound(optionName, commandName, keyType));
            }
            return keyConverter;
        }

        private static IConverter GetConverterFromAttribute(PropertyInfo propertyInfo, string optionName, string commandName)
        {
            var converterAttribute = propertyInfo.GetCustomAttributes(typeof(ConverterAttribute), true).FirstOrDefault() as ConverterAttribute;
            if (converterAttribute != null)
            {
                var converter = converterAttribute.BuildConverter();

                if (!converter.CanConvertTo(propertyInfo.PropertyType))
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(optionName, commandName, propertyInfo.PropertyType, converter.TargetType));
                }
                return converter;
            }
            return null;
        }

        private static IConverter GetKeyValueConverterFromAttribute(PropertyInfo propertyInfo, string optionName, string commandName)
        {
            var converterKeyValuePairAttribute = propertyInfo.GetCustomAttributes(typeof(ConverterKeyValueAttribute), true).FirstOrDefault() as ConverterKeyValueAttribute;
            if (converterKeyValuePairAttribute != null)
            {
                if (!propertyInfo.PropertyType.IsDictionaryType())
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractConverterKeyValueConverterIsForIDictionaryProperty(optionName, commandName));
                }
                var keyConverter = GetKeyConverter(propertyInfo, optionName, commandName, converterKeyValuePairAttribute);
                var valueConverter = GetValueConverter(propertyInfo, optionName, commandName, converterKeyValuePairAttribute);

                return new KeyValueConverter(keyConverter, valueConverter);
            }
            return null;
        }

        private static IConverter GetValueConverter(PropertyInfo propertyInfo, string optionName, string commandName, ConverterKeyValueAttribute converterKeyValuePairAttribute)
        {
            var valueType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(false);
            var valueConverter = converterKeyValuePairAttribute.BuildValueConverter();
            if (!valueType.IsType(valueConverter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractValueConverterIsNotValid(optionName, commandName, valueType, valueConverter.TargetType));
            }
            return valueConverter;
        }

        private static IConverter GetKeyConverter(PropertyInfo propertyInfo, string optionName, string commandName, ConverterKeyValueAttribute converterKeyValuePairAttribute)
        {
            var keyType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(true);
            var keyConverter = converterKeyValuePairAttribute.BuildKeyConverter();
            if (!keyType.IsType(keyConverter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractKeyConverterIsNotValid(optionName, commandName, keyType, keyConverter.TargetType));
            }

            return keyConverter;
        }

        private static IConverter GetConverterFromType(Type type, IEnumerable<IConverter> converters)
        {
            var converter = (from kvp in converters
                             where kvp.CanConvertTo(type)
                             select kvp).FirstOrDefault();
            return converter;
        }

        internal static bool ExtractIsRequiredMetadata(this PropertyInfo source)
        {
            Guard.NotNull(source, nameof(source));

            var requiredAttribute = source.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
            return requiredAttribute != null;
        }

        [NotNull]
        internal static OptionDisplayInfo ExtractOptionDisplayInfoMetadata(this PropertyInfo source)
        {
            Guard.NotNull(source, nameof(source));
            var optionDisplayInfo = new OptionDisplayInfo
            {
                Name = source.Name,
                ShortName = source.Name,
                Description = ""
            };
            var displayAttribute = source.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttribute != null)
            {
                optionDisplayInfo.Name = displayAttribute.GetName() ?? source.Name;
                optionDisplayInfo.ShortName = displayAttribute.GetShortName();
                optionDisplayInfo.Description = displayAttribute.GetDescription();
            }
            var nameAsKebabCase = optionDisplayInfo.Name.AsKebabCase();
            if (!nameAsKebabCase.Equals(optionDisplayInfo.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                optionDisplayInfo.AlternateNames = new[] { nameAsKebabCase };
            }
            return optionDisplayInfo;
        }

        internal static object ExtractDefaultValue([NotNull] this PropertyInfo source, [NotNull] Func<object, object> valueConverter)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(valueConverter, nameof(valueConverter));

            if (!source.PropertyType.IsMultiValuedType())
            {
                var defaultValueAttribute = source.GetCustomAttributes(typeof(DefaultValueAttribute), true).OfType<DefaultValueAttribute>().FirstOrDefault();
                var originalDefaultValue = defaultValueAttribute?.Value;
                var defaultValue = valueConverter(originalDefaultValue);
                return defaultValue;
            }
            return null;
        }

        /// <summary>
        /// Indicates if the given <see cref="PropertyInfo"/> should be ignored as option by the parse.
        /// </summary>
        /// <param name="source">The <see cref="PropertyInfo"/>.</param>
        /// <returns>true if the <see cref="PropertyInfo"/> should be ignored, false otherwise.</returns>
        internal static bool ShouldBeIgnored(this PropertyInfo source)
        {
            Guard.NotNull(source, nameof(source));

            return source.GetCustomAttributes(typeof(IgnoreOptionPropertyAttribute), true).Any();
        }
    }
}