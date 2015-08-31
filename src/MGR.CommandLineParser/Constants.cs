using System;
using System.Globalization;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    internal static class Constants
    {
        internal static class ExceptionMessages
        {
            internal static readonly Func<object, Type, string> FormatConverterUnableConvert = (o, t) => string.Format(CultureInfo.InvariantCulture, "Unable to parse '{0}' to type '{1}'.", o, t.Name);
            internal static readonly Func<object, string, string> FormatParserOptionNotFoundForCommand = (commandName, optionName) => string.Format(CultureInfo.InvariantCulture, "There is no option '{1}' for the command '{0}'.", commandName, optionName);
            internal static readonly Func<object, string, string> FormatParserOptionValueRequired = (commandName, optionName) => string.Format(CultureInfo.InvariantCulture, "You should specified a value for the option '{1}' of the command '{0}'.", commandName, optionName);
            internal static readonly string NoValidCommand = "The command is not in a valid state.";
            // ReSharper disable once InconsistentNaming
            internal static readonly string IConverterTypeName = typeof (IConverter).FullName;
            internal static readonly string ConverterKeyConverterTypeMustBeIConverter = "The key converter type must implement " + IConverterTypeName;
            internal static readonly string ConverterValueConverterTypeMustBeIConverter = "The value converter type must implement " + IConverterTypeName;
            internal static readonly string ConverterAttributeTypeMustBeIConverter = "The converter type must implement " + IConverterTypeName;
        }

        internal static readonly string[] ArgumentStarter = { "/", "-" };
        internal static readonly char ArgumentSplitter = ':';
    }
}