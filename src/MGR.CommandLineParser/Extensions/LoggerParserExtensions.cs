namespace Microsoft.Extensions.Logging;

internal static partial class LoggerParserExtensions
{
    private enum ParserEventId
    {
        CreationOfParserEngine = 1000,
        ParseForSpecificCommandType,
        NoCommandFoundAfterSpecificParsing,
        CommandFoundAfterSpecificParsing,
        ParseWithDefaultCommandType,
        ArgumentProvidedWithDefaultCommandType,
        CommandFoundWithDefaultCommandType,
        NoCommandFoundWithDefaultCommandType,
        NoArgumentProvidedWithDefaultCommandType,
        ParseForNotAlreadyKnownCommand,
        NoCommandNameForNotAlreadyKnownCommand,
        ParseUsingCommandName,
        NoCommandTypeFoundForNotAlreadyKnownCommand,
        CommandTypeFoundForNotAlreadyKnownCommand,
        ParsedCommandIsNotValid
    }
}
