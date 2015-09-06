using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;

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

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ICollection")]
        internal static OptionMetadataTemplate ExtractMetadata(this PropertyInfo source, CommandMetadataTemplate commandMetadataTemplate)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(commandMetadataTemplate, nameof(commandMetadataTemplate));

            if (source.ShouldBeIgnored())
            {
                return null;
            }
            if (!source.IsValidOptionProperty())
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractMetadataPropertyShouldBeWritableOrICollection(source.Name, commandMetadataTemplate.Name));
            }
            var metadata = CreateOptionMetadataTemplate(source, commandMetadataTemplate);
            return metadata;
        }
        private static OptionMetadataTemplate CreateOptionMetadataTemplate(PropertyInfo source, CommandMetadataTemplate commandMetadataTemplate)
        {
            var metadata = new OptionMetadataTemplate(source, commandMetadataTemplate);

            metadata = source.ExtractDisplayMetadata(metadata);

            metadata = source.ExtractRequiredMetadata(metadata);

            metadata = source.ExtractConverterMetadata(metadata);

            metadata = source.ExtractDefaultValue(metadata);

            return metadata;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TValue"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TKey"),
         SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "KeyValueConverter"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IDictionary")]
        internal static OptionMetadataTemplate ExtractConverterMetadata(this PropertyInfo source, OptionMetadataTemplate optionMetadataTemplate)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(optionMetadataTemplate, nameof(optionMetadataTemplate));

            var converter = GetConverterFromAttribute(source, optionMetadataTemplate)
    ?? GetKeyValueConverterFromAttribute(source, optionMetadataTemplate)
    ?? FindConverter(source, optionMetadataTemplate);
            if (converter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoConverterFound(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, source.PropertyType));
            }
            optionMetadataTemplate.Converter = converter;

            return optionMetadataTemplate;
        }
        private static IConverter FindConverter(PropertyInfo propertyInfo, OptionMetadataTemplate optionMetadataTemplate)
        {
            var converters = ServiceResolver.Current.ResolveServices<IConverter>().ToList();
            if (propertyInfo.PropertyType.IsDictionaryType())
            {
                var keyConverter = FindKeyConverter(propertyInfo, converters, optionMetadataTemplate);
                var valueConverter = FindValueConverter(propertyInfo, converters, optionMetadataTemplate);

                return new KeyValueConverter(keyConverter, valueConverter);
            }
            var converter = GetConverterFromType(propertyInfo.PropertyType, converters);
            return converter;
        }
        private static IConverter FindValueConverter(PropertyInfo propertyInfo, IEnumerable<IConverter> converters, OptionMetadataTemplate optionMetadataTemplate)
        {
            var valueType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(false);
            var valueConverter = GetConverterFromType(valueType, converters);
            if (valueConverter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoValueConverterFound(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, valueType));
            }
            return valueConverter;
        }
        private static IConverter FindKeyConverter(PropertyInfo propertyInfo, IEnumerable<IConverter> converters, OptionMetadataTemplate optionMetadataTemplate)
        {
            var keyType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(true);
            var keyConverter = GetConverterFromType(keyType, converters);
            if (keyConverter == null)
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserNoKeyConverterFound(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, keyType));
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

        private static IConverter GetConverterFromAttribute(PropertyInfo propertyInfo, OptionMetadataTemplate optionMetadataTemplate)
        {
            var converterAttribute = propertyInfo.GetCustomAttributes(typeof(ConverterAttribute), true).FirstOrDefault() as ConverterAttribute;
            if (converterAttribute != null)
            {
                var converter = converterAttribute.BuildConverter();

                if (!converter.CanConvertTo(propertyInfo.PropertyType))
                {
                    throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "The specified converter for the option '{0}' of the command '{1}' is not valid : property type : {2}, converter target type : {3}.",
                                                                       optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, propertyInfo.PropertyType.FullName, converter.TargetType.FullName));
                }
                return converter;
            }
            return null;
        }

        private static IConverter GetKeyValueConverterFromAttribute(PropertyInfo propertyInfo, OptionMetadataTemplate optionMetadataTemplate)
        {
            var converterKeyValuePairAttribute = propertyInfo.GetCustomAttributes(typeof(ConverterKeyValueAttribute), true).FirstOrDefault() as ConverterKeyValueAttribute;
            if (converterKeyValuePairAttribute != null)
            {
                if (!propertyInfo.PropertyType.IsDictionaryType())
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractConverterKeyValueConverterIsForIDictionaryProperty(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name));
                }
                var keyConverter = GetKeyConverter(propertyInfo, optionMetadataTemplate, converterKeyValuePairAttribute);
                var valueConverter = GetValueConverter(propertyInfo, optionMetadataTemplate, converterKeyValuePairAttribute);

                return new KeyValueConverter(keyConverter, valueConverter);
            }
            return null;
        }

        private static IConverter GetValueConverter(PropertyInfo propertyInfo, OptionMetadataTemplate optionMetadataTemplate, ConverterKeyValueAttribute converterKeyValuePairAttribute)
        {
            var valueType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(false);
            var valueConverter = converterKeyValuePairAttribute.BuildValueConverter();
            if (!valueType.IsAssignableFrom(valueConverter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractValueConverterIsNotValid(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, valueType, valueConverter.TargetType));
            }
            return valueConverter;
        }

        private static IConverter GetKeyConverter(PropertyInfo propertyInfo, OptionMetadataTemplate optionMetadataTemplate, ConverterKeyValueAttribute converterKeyValuePairAttribute)
        {
            var keyType = propertyInfo.PropertyType.GetUnderlyingDictionaryType(true);
            var keyConverter = converterKeyValuePairAttribute.BuildKeyConverter();
            if (!keyType.IsAssignableFrom(keyConverter.TargetType))
            {
                throw new CommandLineParserException(Constants.ExceptionMessages.ParserExtractKeyConverterIsNotValid(optionMetadataTemplate.Name, optionMetadataTemplate.CommandMetadata.Name, keyType, keyConverter.TargetType));
            }

            return keyConverter;
        }

        internal static OptionMetadataTemplate ExtractRequiredMetadata(this PropertyInfo source, OptionMetadataTemplate optionMetadataTemplate)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(optionMetadataTemplate, nameof(optionMetadataTemplate));

            var requiredAttribute = source.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
            if (requiredAttribute != null)
            {
                optionMetadataTemplate.IsRequired = true;
            }
            return optionMetadataTemplate;
        }

        internal static OptionMetadataTemplate ExtractDisplayMetadata(this PropertyInfo source, OptionMetadataTemplate optionMetadataTemplate)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(optionMetadataTemplate, nameof(optionMetadataTemplate));

            var displayAttribute = source.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttribute == null)
            {
                optionMetadataTemplate.Name = source.Name;
                optionMetadataTemplate.ShortName = source.Name;
            }
            else
            {
                optionMetadataTemplate.Name = displayAttribute.GetName() ?? source.Name;
                var shortName = displayAttribute.GetShortName();
                if (!string.IsNullOrEmpty(shortName))
                {
                    optionMetadataTemplate.ShortName = shortName;
                }
                optionMetadataTemplate.Description = displayAttribute.GetDescription();
            }
            return optionMetadataTemplate;
        }

        internal static OptionMetadataTemplate ExtractDefaultValue(this PropertyInfo source, OptionMetadataTemplate optionMetadataTemplate)
        {
            Guard.NotNull(source, nameof(source));
            Guard.NotNull(optionMetadataTemplate, nameof(optionMetadataTemplate));

            if (!source.PropertyType.IsMultiValuedType())
            {
                var defaultValueAttribute = source.GetCustomAttributes(typeof(DefaultValueAttribute), true).OfType<DefaultValueAttribute>().FirstOrDefault();
                var defaultValue = defaultValueAttribute?.Value;
                optionMetadataTemplate.DefaultValue = ConvertDefaultValue(optionMetadataTemplate, defaultValue);
            }
            return optionMetadataTemplate;
        }

        private static object ConvertDefaultValue(OptionMetadataTemplate optionMetadataTemplate, object defaultValue)
        {
            if (defaultValue != null)
            {
                if (optionMetadataTemplate.OptionType != defaultValue.GetType())
                {
                    var conververtedDefaultValue = optionMetadataTemplate.Converter.Convert(defaultValue.ToString(), optionMetadataTemplate.OptionType);
                    return conververtedDefaultValue;
                }
            }
            return defaultValue;
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