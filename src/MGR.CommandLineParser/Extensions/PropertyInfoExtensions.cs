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
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.CanWrite || source.PropertyType.IsMultiValuedType();
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ICollection")]
        internal static OptionMetadataTemplate ExtractMetadata(this PropertyInfo propertySource, CommandMetadataTemplate commandMetadataTemplate)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            if (commandMetadataTemplate == null)
            {
                throw new ArgumentNullException(nameof(commandMetadataTemplate));
            }
            if (propertySource.ShouldBeIgnored())
            {
                return null;
            }
            if (!propertySource.IsValidOptionProperty())
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "The option '{0}' of the command '{1}' must be writable or implements ICollection<T>.",
                                                                   propertySource.Name, commandMetadataTemplate.Name));
            }
            var metadata = new OptionMetadataTemplate(propertySource, commandMetadataTemplate);

            metadata = propertySource.ExtractDisplayMetadata(metadata);

            metadata = propertySource.ExtractRequiredMetadata(metadata);

            metadata = propertySource.ExtractConverterMetadata(metadata);

            metadata = propertySource.ExtractDefaultValue(metadata);

            return metadata;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TValue"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TKey"),
         SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "KeyValueConverter"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IDictionary")]
        internal static OptionMetadataTemplate ExtractConverterMetadata(this PropertyInfo propertySource, OptionMetadataTemplate metadata)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            var converterAttribute = propertySource.GetCustomAttributes(typeof(ConverterAttribute), true).FirstOrDefault() as ConverterAttribute;
            if (converterAttribute != null)
            {
                var converter = converterAttribute.BuildConverter();

                if (!converter.CanConvertTo(propertySource.PropertyType))
                {
                    throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "The specified converter for the option '{0}' of the command '{1}' is not valid : property type : {2}, converter target type : {3}.",
                                                                       metadata.Name, metadata.CommandMetadata.Name, propertySource.PropertyType.FullName, converter.TargetType.FullName));
                }
                metadata.Converter = converter;
            }
            else
            {
                var converterKeyValuePairAttribute = propertySource.GetCustomAttributes(typeof(ConverterKeyValueAttribute), true).FirstOrDefault() as ConverterKeyValueAttribute;
                if (converterKeyValuePairAttribute != null)
                {
                    if (!propertySource.PropertyType.IsDictionaryType())
                    {
                        throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "The option '{0}' of the command '{1}' defined a Key/Value converter but its type is not System.Generic.IDictionary<TKey, TValue>.",
                                                                           metadata.Name, metadata.CommandMetadata.Name));
                    }
                    var keyType = propertySource.PropertyType.GetUnderlyingDictionaryType(true);
                    var keyConverter = converterKeyValuePairAttribute.BuildKeyConverter();
                    if (!keyType.IsAssignableFrom(keyConverter.TargetType))
                    {
                        throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture,
                                                                           "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : key property type : {2}, key converter target type : {3}.",
                                                                           metadata.Name, metadata.CommandMetadata.Name, keyType.FullName, keyConverter.TargetType.FullName));
                    }

                    var valueType = propertySource.PropertyType.GetUnderlyingDictionaryType(false);
                    var valueConverter = converterKeyValuePairAttribute.BuildValueConverter();
                    if (!valueType.IsAssignableFrom(valueConverter.TargetType))
                    {
                        throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture,
                                                                           "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : value property type : {2}, value converter target type : {3}.",
                                                                           metadata.Name, metadata.CommandMetadata.Name, valueType.FullName, valueConverter.TargetType.FullName));
                    }
                    metadata.Converter = new KeyValueConverter(keyConverter, valueConverter);
                }
                else
                {
                    var converters = ServiceResolver.Current.ResolveServices<IConverter>().ToList();
                    if (propertySource.PropertyType.IsDictionaryType())
                    {
                        var keyType = propertySource.PropertyType.GetUnderlyingDictionaryType(true);
                        var keyConverter = (from kvp in converters
                                                   where kvp.CanConvertTo(keyType)
                                                   select kvp).FirstOrDefault();
                        if (keyConverter == null)
                        {
                            throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "No converter found for the key type ('{2}') of the option '{0}' of the command '{1}'.",
                                                                               metadata.Name, metadata.CommandMetadata.Name, keyType.FullName));
                        }

                        var valueType = propertySource.PropertyType.GetUnderlyingDictionaryType(false);
                        var valueConverter = (from kvp in converters
                                                     where kvp.CanConvertTo(valueType)
                                                     select kvp).FirstOrDefault();
                        if (valueConverter == null)
                        {
                            throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "No converter found for the value type ('{2}') of the option '{0}' of the command '{1}'.",
                                                                               metadata.Name, metadata.CommandMetadata.Name, valueType.FullName));
                        }
                        metadata.Converter = new KeyValueConverter(keyConverter, valueConverter);
                    }
                    else
                    {
                        var converter = (from kvp in converters
                                                where kvp.CanConvertTo(propertySource.PropertyType)
                                                select kvp).FirstOrDefault();

                        if (converter == null)
                        {
                            throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "No converter found for the option '{0}' of the command '{1}' of type '{2}'.", metadata.Name, metadata.CommandMetadata.Name,
                                                                               propertySource.PropertyType.FullName));
                        }
                        metadata.Converter = converter;
                    }
                }
            }

            return metadata;
        }

        internal static OptionMetadataTemplate ExtractRequiredMetadata(this PropertyInfo propertySource, OptionMetadataTemplate metadata)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            var requiredAttribute = propertySource.GetCustomAttributes(typeof(RequiredAttribute), true).FirstOrDefault() as RequiredAttribute;
            if (requiredAttribute != null)
            {
                metadata.IsRequired = true;
            }
            return metadata;
        }

        internal static OptionMetadataTemplate ExtractDisplayMetadata(this PropertyInfo propertySource, OptionMetadataTemplate metadata)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            var displayAttribute = propertySource.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            if (displayAttribute == null)
            {
                metadata.Name = propertySource.Name;
                metadata.ShortName = propertySource.Name;
            }
            else
            {
                metadata.Name = displayAttribute.GetName() ?? propertySource.Name;
                var shortName = displayAttribute.GetShortName();
                if (!string.IsNullOrEmpty(shortName))
                {
                    metadata.ShortName = shortName;
                }
                metadata.Description = displayAttribute.GetDescription();
            }
            return metadata;
        }

        internal static OptionMetadataTemplate ExtractDefaultValue(this PropertyInfo propertySource, OptionMetadataTemplate metadata)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            if (!propertySource.PropertyType.IsMultiValuedType())
            {
                var defaultValueAttribute = propertySource.GetCustomAttributes(typeof(DefaultValueAttribute), true).OfType<DefaultValueAttribute>().FirstOrDefault();
                if (defaultValueAttribute != null)
                {
                    var defaultValue = defaultValueAttribute.Value;
                    if (defaultValue != null)
                    {
                        if (metadata.OptionType == defaultValue.GetType())
                        {
                            metadata.DefaultValue = defaultValue;
                        }
                        else
                        {
                            var conververtedDefaultValue = metadata.Converter.Convert(defaultValue.ToString(), metadata.OptionType);
                            metadata.DefaultValue = conververtedDefaultValue;
                        }
                    }
                }
            }
            return metadata;
        }

        /// <summary>
        /// Indicates if the given <see cref="PropertyInfo"/> should be ignored as option by the parse.
        /// </summary>
        /// <param name="propertySource">The <see cref="PropertyInfo"/>.</param>
        /// <returns>true if the <see cref="PropertyInfo"/> should be ignored, false otherwise.</returns>
        internal static bool ShouldBeIgnored(this PropertyInfo propertySource)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }
            return propertySource.GetCustomAttributes(typeof(IgnoreOptionPropertyAttribute), true).Any();
        }
    }
}