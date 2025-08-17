using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MGR.CommandLineParser;
using MGR.CommandLineParser.Diagnostics;

namespace Microsoft.Extensions.Logging;

internal static partial class LoggerParserExtensions
{
    private static readonly Action<ILogger<LoggerCategory.Parser>, Exception?> CreationOfParserEngineAction = LoggerMessage.Define(LogLevel.Debug, CreateParserId(ParserEventId.CreationOfParserEngine), "Creation of the parser engine");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Type, Exception?> ParseForSpecificCommandTypeAction = LoggerMessage.Define<Type>(LogLevel.Information, CreateParserId(ParserEventId.ParseForSpecificCommandType), "Parse for a specific command type: '{SpecificCommandType}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Exception?> NoCommandFoundAfterSpecificParsingAction = LoggerMessage.Define(LogLevel.Warning, CreateParserId(ParserEventId.NoCommandFoundAfterSpecificParsing), "No command found after parsing a specific type");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Type, CommandParsingResultCode, int, Exception?> CommandFoundAfterSpecificParsingAction = LoggerMessage.Define<Type, CommandParsingResultCode, int>(LogLevel.Information, CreateParserId(ParserEventId.CommandFoundAfterSpecificParsing), "Command found after parsing a specific type: Type '{CommandType}', ParsingResultCode: {ParsingResultCode}, Number of failed validations: {NbFailedValidations}");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Type, Exception?> ParseWithDefaultCommandTypeAction = LoggerMessage.Define<Type>(LogLevel.Information, CreateParserId(ParserEventId.ParseWithDefaultCommandType), "Parsing with default command type: '{DefaultCommandType}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Exception?> ArgumentProvidedWithDefaultCommandTypeAction = LoggerMessage.Define<string>(LogLevel.Information, CreateParserId(ParserEventId.ArgumentProvidedWithDefaultCommandType), "Argument provided that can be command name: '{CommandName}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Exception?> CommandFoundWithDefaultCommandTypeAction = LoggerMessage.Define<string>(LogLevel.Information, CreateParserId(ParserEventId.CommandFoundWithDefaultCommandType), "Command type found for command name '{CommandName}' when parsing with default command type");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Type, Exception?> NoCommandFoundWithDefaultCommandTypeAction = LoggerMessage.Define<string, Type>(LogLevel.Information, CreateParserId(ParserEventId.NoCommandFoundWithDefaultCommandType), "No command found corresponding to commandName: '{CommandName}' when parsing with default command type. Fallback to parsing with specific command using the default command type '{DefaultCommandType}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Type, Exception?> NoArgumentProvidedWithDefaultCommandTypeAction = LoggerMessage.Define<Type>(LogLevel.Information, CreateParserId(ParserEventId.NoArgumentProvidedWithDefaultCommandType), "No arguments provided to parse with default command. Fallback to parsing specific command using the default command type '{DefaultCommandType}', but without additional arguments.");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Exception?> ParseForNotAlreadyKnownCommandAction = LoggerMessage.Define(LogLevel.Information, CreateParserId(ParserEventId.ParseForNotAlreadyKnownCommand), "Parse arguments for not already known command.");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Exception?> NoCommandNameForNotAlreadyKnownCommandAction = LoggerMessage.Define(LogLevel.Error, CreateParserId(ParserEventId.NoCommandNameForNotAlreadyKnownCommand), "No command name provided for parsing not already known command.");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Exception?> ParseUsingCommandNameAction =
        LoggerMessage.Define<string>(LogLevel.Information, CreateParserId(ParserEventId.ParseUsingCommandName), "Parse using command name '{CommandName}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Exception?> NoCommandTypeFoundForNotAlreadyKnownCommandAction =
        LoggerMessage.Define<string>(LogLevel.Error, CreateParserId(ParserEventId.NoCommandTypeFoundForNotAlreadyKnownCommand), "No CommandType found for the command name '{CommandName}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, string, Exception?> CommandTypeFoundForNotAlreadyKnownCommandAction =
        LoggerMessage.Define<string>(LogLevel.Information, CreateParserId(ParserEventId.CommandTypeFoundForNotAlreadyKnownCommand), "CommandType found for the command name '{CommandName}'");
    private static readonly Action<ILogger<LoggerCategory.Parser>, Exception?> ParsedCommandIsNotValidAction =
        LoggerMessage.Define(LogLevel.Warning, CreateParserId(ParserEventId.ParsedCommandIsNotValid), "Parsed command is not valid");

    private static EventId CreateParserId(ParserEventId eventId) => new EventId((int)eventId, LoggerCategory.Parser.Name + "." + (int)eventId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CreationOfParserEngine(this ILogger<LoggerCategory.Parser> logger)
    {
        CreationOfParserEngineAction(logger, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoCommandFoundAfterSpecificParsing(this ILogger<LoggerCategory.Parser> logger)
    {
        NoCommandFoundAfterSpecificParsingAction(logger, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CommandFoundAfterSpecificParsing(this ILogger<LoggerCategory.Parser> logger, Type commandType, CommandParsingResultCode parsingResultCode, IEnumerable<ValidationResult> validationResults)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            CommandFoundAfterSpecificParsingAction(logger, commandType, parsingResultCode, validationResults.Count(), null);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParseForSpecificCommandType(this ILogger<LoggerCategory.Parser> logger, Type specificCommandType)
    {
        ParseForSpecificCommandTypeAction(logger, specificCommandType, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParseWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, Type defaultCommandType)
    {
        ParseWithDefaultCommandTypeAction(logger, defaultCommandType, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentProvidedWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, string commandName)
    {
        ArgumentProvidedWithDefaultCommandTypeAction(logger, commandName, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CommandTypeFoundWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, string commandName)
    {
        CommandFoundWithDefaultCommandTypeAction(logger, commandName, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoCommandTypeFoundWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, string commandName, Type defaultCommandType)
    {
        NoCommandFoundWithDefaultCommandTypeAction(logger, commandName, defaultCommandType, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoArgumentProvidedWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, Type defaultCommandType)
    {
        NoArgumentProvidedWithDefaultCommandTypeAction(logger, defaultCommandType, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParseForNotAlreadyKnownCommand(this ILogger<LoggerCategory.Parser> logger) =>
        ParseForNotAlreadyKnownCommandAction(logger, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoCommandNameForNotAlreadyKnownCommand(this ILogger<LoggerCategory.Parser> logger) =>
        NoCommandNameForNotAlreadyKnownCommandAction(logger, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParseUsingCommandName(this ILogger<LoggerCategory.Parser> logger, string commandName) =>
        ParseUsingCommandNameAction(logger, commandName, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NoCommandTypeFoundForNotAlreadyKnownCommand(this ILogger<LoggerCategory.Parser> logger, string commandName) =>
        NoCommandTypeFoundForNotAlreadyKnownCommandAction(logger, commandName, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CommandTypeFoundForNotAlreadyKnownCommand(this ILogger<LoggerCategory.Parser> logger, string commandName) =>
        CommandTypeFoundForNotAlreadyKnownCommandAction(logger, commandName, null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParsedCommandIsNotValid(this ILogger<LoggerCategory.Parser> logger) =>
        ParsedCommandIsNotValidAction(logger, null);
}
