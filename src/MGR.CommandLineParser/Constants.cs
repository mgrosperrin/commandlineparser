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
        }
    }
}