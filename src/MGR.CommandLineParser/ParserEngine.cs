using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MGR.CommandLineParser
{
    internal class ParserEngine
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LoggerCategory.Parser> _logger;

        internal ParserEngine(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
        }

        internal async Task<ParsingResult> Parse<TCommand>(IEnumerator<string> argumentsEnumerator) where TCommand : class, ICommand
        {
            using (_logger.BeginParsingForSpecificCommandType(typeof(TCommand)))
            {
                var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
                var commandType = await commandTypeProviders.GetCommandType<TCommand>();
                var parsingResult = ParseImpl(argumentsEnumerator, commandType);
                if (parsingResult.ParsingResultCode == CommandParsingResultCode.NoCommandFound)
                {
                    _logger.NoCommandFoundAfterSpecificParsing();
                }
                else
                {
                    _logger.CommandFoundAfterSpecificParsing(typeof(TCommand), parsingResult.ParsingResultCode,
                        parsingResult.ValidationResults);
                }

                return parsingResult;
            }
        }

        internal async Task<ParsingResult> ParseWithDefaultCommand<TCommand>(IEnumerator<string> argumentsEnumerator)
            where TCommand : class, ICommand
        {
            using (_logger.BeginParsingWithDefaultCommandType(typeof(TCommand)))
            {
                var commandName = argumentsEnumerator.GetNextCommandLineItem();
                if (commandName != null)
                {
                    _logger.ArgumentProvidedWithDefaultCommandType(commandName);
                    var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
                    var commandType = await commandTypeProviders.GetCommandType(commandName);
                    if (commandType != null)
                    {
                        _logger.CommandTypeFoundWithDefaultCommandType(commandName);
                        return ParseImpl(argumentsEnumerator, commandType);
                    }

                    _logger.NoCommandTypeFoundWithDefaultCommandType(commandName, typeof(TCommand));
                    var withArgumentsCommandResult = await Parse<TCommand>(argumentsEnumerator.PrefixWith(commandName));
                    return withArgumentsCommandResult;

                }

                _logger.NoArgumentProvidedWithDefaultCommandType(typeof(TCommand));
                var noArgumentsCommandResult = await Parse<TCommand>(argumentsEnumerator);
                return noArgumentsCommandResult;
            }
        }

        internal async Task<ParsingResult> Parse(IEnumerator<string> argumentsEnumerator)
        {
            _logger.ParseForNotAlreadyKnownCommand();
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName == null)
            {
                _logger.NoCommandNameForNotAlreadyKnownCommand();
                var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
                await helpWriter.WriteCommandListing();
                return new ParsingResult(null, null, CommandParsingResultCode.NoCommandNameProvided);
            }

            using (_logger.BeginParsingUsingCommandName(commandName))
            {
                var commandTypeProviders = _serviceProvider.GetServices<ICommandTypeProvider>();
                var commandType = await commandTypeProviders.GetCommandType(commandName);
                if (commandType == null)
                {
                    _logger.NoCommandTypeFoundForNotAlreadyKnownCommand(commandName);
                    var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
                    await helpWriter.WriteCommandListing();
                    return new ParsingResult(null, null, CommandParsingResultCode.NoCommandFound);
                }

                _logger.CommandTypeFoundForNotAlreadyKnownCommand(commandName);
                return ParseImpl(argumentsEnumerator, commandType);
            }

        }

        private ParsingResult ParseImpl(IEnumerator<string> argumentsEnumerator, ICommandType commandType)
        {
            var commandObjectBuilder = ExtractCommandLineOptions(commandType, argumentsEnumerator);
            if (commandObjectBuilder == null)
            {
                return new ParsingResult(null, null, CommandParsingResultCode.CommandParametersNotValid);
            }
            var validation = commandObjectBuilder.Validate(_serviceProvider);
            if (!validation.IsValid)
            {
                _logger.ParsedCommandIsNotValid();
                var helpWriter = _serviceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteHelpForCommand(commandType);
                return new ParsingResult(commandObjectBuilder.GenerateCommandObject(), validation.ValidationErrors, CommandParsingResultCode.CommandParametersNotValid);
            }
            return new ParsingResult(commandObjectBuilder.GenerateCommandObject(), null, CommandParsingResultCode.Success);
        }
        private ICommandObjectBuilder ExtractCommandLineOptions(ICommandType commandType, IEnumerator<string> argumentsEnumerator)
        {
            var commandObjectBuilder = commandType.CreateCommandObjectBuilder(_serviceProvider);
            if (commandObjectBuilder == null)
            {
                return null;
            }
            var alwaysPutInArgumentList = false;
            while (true)
            {
                var argument = argumentsEnumerator.GetNextCommandLineItem();
                if (argument == null)
                {
                    break;
                }
                if (argument.Equals(Constants.EndOfOptions))
                {
                    alwaysPutInArgumentList = true;
                    continue;
                }

                if (alwaysPutInArgumentList || !argument.StartsWith(StringComparison.OrdinalIgnoreCase, Constants.OptionStarter))
                {
                    commandObjectBuilder.AddArguments(argument);
                    continue;
                }

                var starterLength = 2;
                Func<ICommandObjectBuilder, string, ICommandOption> commandOptionFinder = (co, optionName) => co.FindOption(optionName);
                if (!argument.StartsWith(Constants.LongNameOptionStarter))
                {
                    starterLength = Constants.ShortNameOptionStarter.Length;
                    commandOptionFinder = (co, optionName) => co.FindOptionByShortName(optionName);
                }
                var optionText = argument.Substring(starterLength);
                string value = null;
                var splitIndex = optionText.IndexOf(Constants.OptionSplitter);
                if (splitIndex > 0)
                {
                    value = optionText.Substring(splitIndex + 1);
                    optionText = optionText.Substring(0, splitIndex);
                }

                var option = commandOptionFinder(commandObjectBuilder, optionText);
                if (option == null)
                {
                    var console = _serviceProvider.GetRequiredService<IConsole>();
                    console.WriteLineError(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandType.Metadata.Name, optionText));
                    return null;
                }

                if (option.ShouldProvideValue)
                {
                    value = value ?? argumentsEnumerator.GetNextCommandLineItem();
                }

                option.AssignValue(value);
            }
            return commandObjectBuilder;
        }
    }
}
