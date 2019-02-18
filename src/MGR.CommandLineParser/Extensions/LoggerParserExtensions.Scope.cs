using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MGR.CommandLineParser.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    internal static partial class LoggerParserExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDisposable BeginParsingArguments(this ILogger<LoggerCategory.Parser> logger, string correlationId) => logger.BeginScope(new Dictionary<string, object>{{"ParsingId", correlationId}});

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDisposable BeginParsingForSpecificCommandType(this ILogger<LoggerCategory.Parser> logger, Type specificCommandType)
        {
            var scope = logger.BeginScope(new Dictionary<string, object> { { "SpecificCommandType", specificCommandType } });
            logger.ParseForSpecificCommandType(specificCommandType);
            return scope;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDisposable BeginParsingWithDefaultCommandType(this ILogger<LoggerCategory.Parser> logger, Type defaultCommandType)
        {
            var scope = logger.BeginScope(new Dictionary<string, object> { { "DefaultCommandType", defaultCommandType } });
            logger.ParseWithDefaultCommandType(defaultCommandType);
            return scope;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDisposable BeginParsingUsingCommandName(this ILogger<LoggerCategory.Parser> logger, string commandName)
        {
            var scope = logger.BeginScope(new Dictionary<string, object> { { "CommandName", commandName } });
            logger.ParseUsingCommandName(commandName);
            return scope;
        }
    }
}
