using System;
using System.Globalization;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal static class Constants
    {
        internal static readonly string[] ArgumentStarter = {"/", "-"};
        internal static readonly char ArgumentSplitter = ':';

        internal static class ExceptionMessages
        {
            internal static readonly Func<object, Type, string> FormatConverterUnableConvert = (o, t) => string.Format(CultureInfo.InvariantCulture, "Unable to parse '{0}' to type '{1}'.", o, t.Name);

            internal static readonly Func<object, string, string> FormatParserOptionNotFoundForCommand =
                (commandName, optionName) => string.Format(CultureInfo.InvariantCulture, "There is no option '{1}' for the command '{0}'.", commandName, optionName);

            internal static readonly Func<object, string, string> FormatParserOptionValueRequired =
                (commandName, optionName) => string.Format(CultureInfo.InvariantCulture, "You should specified a value for the option '{1}' of the command '{0}'.", commandName, optionName);

            internal static readonly string NoValidCommand = "The command is not in a valid state.";
            // ReSharper disable once InconsistentNaming
            internal static readonly string IConverterTypeName = typeof (IConverter).FullName;
            internal static readonly string ConverterKeyConverterTypeMustBeIConverter = "The key converter type must implement " + IConverterTypeName;
            internal static readonly string ConverterValueConverterTypeMustBeIConverter = "The value converter type must implement " + IConverterTypeName;
            internal static readonly string ConverterAttributeTypeMustBeIConverter = "The converter type must implement " + IConverterTypeName;

            internal static readonly Func<string, string, string> ParserExtractMetadataPropertyShouldBeWritableOrICollection =
                (propertyName, commandName) =>
                    string.Format(CultureInfo.CurrentUICulture, "The option '{0}' of the command '{1}' must be writable or implements ICollection<T>.", propertyName, commandName);

            internal static readonly Func<string, string, string> ParserExtractConverterKeyValueConverterIsForIDictionaryProperty =
                (propertyName, commandName) =>
                    string.Format(CultureInfo.CurrentUICulture, "The option '{0}' of the command '{1}' defined a Key/Value converter but its type is not System.Generic.IDictionary<TKey, TValue>.",
                        propertyName, commandName);

            internal static readonly Func<string, string, Type, Type, string> ParserExtractKeyConverterIsNotValid =
                (propertyName, commandName, keyPropertyType, keyConverterTargetType) => string.Format(CultureInfo.CurrentUICulture,
                    "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : key property type : {2}, key converter target type : {3}.", propertyName, commandName,
                    keyPropertyType.FullName, keyConverterTargetType.FullName);

            internal static readonly Func<string, string, Type, Type, string> ParserExtractValueConverterIsNotValid =
                (propertyName, commandName, valuePropertyType, valueConverterTargetType) => string.Format(CultureInfo.CurrentUICulture,
                    "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : value property type : {2}, value converter target type : {3}.", propertyName, commandName,
                    valuePropertyType.FullName, valueConverterTargetType.FullName);

            internal static readonly Func<string, string, Type, string> ParserNoKeyConverterFound = (propertyName, commandName, keyType) =>
                string.Format(CultureInfo.CurrentUICulture, "No converter found for the key type ('{2}') of the option '{0}' of the command '{1}'.", propertyName, commandName, keyType.FullName);

            internal static readonly Func<string, string, Type, string> ParserNoValueConverterFound = (propertyName, commandName, valueType) =>
                string.Format(CultureInfo.CurrentUICulture, "No converter found for the value type ('{2}') of the option '{0}' of the command '{1}'.", propertyName, commandName, valueType.FullName);

            internal static readonly Func<string, string, Type, string> ParserNoConverterFound = (propertyName, commandName, propertyType) =>
                string.Format(CultureInfo.CurrentUICulture, "No converter found for the option '{0}' of the command '{1}' of type '{2}'.", propertyName, commandName, propertyType.FullName);

            internal static readonly Func<Type, Type, string> ParserSpecifiedConverterNotValidToAssignValue =
                (optionType, targetType) => string.Format(CultureInfo.CurrentUICulture, "The specified converter is not valid : target type is '{1}' and option type is '{0}'.", optionType, targetType);

            internal static readonly Func<string, string, Type, Type, string> ParserSpecifiedConverterNotValid =
                (optionName, commandName, optionType, targetType) =>
                    string.Format(CultureInfo.CurrentUICulture, "The specified converter for the option '{0}' of the command '{1}' is not valid : property type : {2}, converter target type : {3}.",
                        optionName, commandName, optionType.FullName, targetType.FullName);

            internal static readonly Func<string, Type, string, string> LocalizableNoPropertyFound =
                (propertyName, resourceType, propertyValue) =>
                    string.Format(CultureInfo.CurrentCulture,
                        "Cannot retrieve property '{0}' because localization failed.  Type '{1}' is not public or does not contain a public static string property with the name '{2}'", propertyName,
                        resourceType.FullName, propertyValue);

            internal static readonly Func<string, string, string> ParserMultiValueOptionIsNullAndHasNoSetter = (optionName, commandName) => string.Format(CultureInfo.CurrentUICulture, "The multi-valued option '{0}' of the command '{1}' returns null and have no setter.", optionName, commandName);

            internal static readonly Func<Type, string> EnumConverterConcreteTargetTypeIsNotAnEnum =
                concreteTargetType => string.Format(CultureInfo.CurrentCulture, "The specified concrete target type ({0}) is not an enum type.", concreteTargetType.FullName);

            internal static readonly Func<string, Type, string> EnumConverterParsedValueIsNotOfConcreteType =
                (value, concreteTargetType) => string.Format(CultureInfo.CurrentCulture, "The specified value '{0}' is not correct the type '{1}'.", value, concreteTargetType);
        }
    }
}