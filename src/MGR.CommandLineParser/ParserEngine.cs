﻿using System;
using System.Collections.Generic;
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
        private readonly IParserOptions _parserOptions;
        private readonly ILogger<LoggerCategory.Parser> _logger;

        public ParserEngine(IParserOptions parserOptions, ILoggerFactory loggerFactory)
        {
            _parserOptions = parserOptions;
            _logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
        }

        public ParsingResult Parse<TCommand>(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator) where TCommand : class, ICommand
        {
            using (_logger.BeginParsingForSpecificCommandType(typeof(TCommand)))
            {
                var commandTypeProvider = serviceProvider.GetRequiredService<ICommandTypeProvider>();
                var commandType = commandTypeProvider.GetCommandType<TCommand>();
                var parsingResult = ParseImpl(argumentsEnumerator, serviceProvider, commandType);
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

        public ParsingResult ParseWithDefaultCommand<TCommand>(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
            where TCommand : class, ICommand
        {
            using (_logger.BeginParsingWithDefaultCommandType(typeof(TCommand)))
            {
                var commandName = argumentsEnumerator.GetNextCommandLineItem();
                if (commandName != null)
                {
                    _logger.ArgumentProvidedWithDefaultCommandType(commandName);
                    var commandTypeProvider = serviceProvider.GetRequiredService<ICommandTypeProvider>();
                    var commandType = commandTypeProvider.GetCommandType(commandName);
                    if (commandType != null)
                    {
                        _logger.CommandTypeFoundWithDefaultCommandType(commandName);
                        return ParseImpl(argumentsEnumerator, serviceProvider, commandType);
                    }

                    _logger.NoCommandTypeFoundWithDefaultCommandType(commandName, typeof(TCommand));
                    var withArgumentsCommandResult = Parse<TCommand>(serviceProvider, argumentsEnumerator.PrefixWith(commandName));
                    return withArgumentsCommandResult;

                }

                _logger.NoArgumentProvidedWithDefaultCommandType(typeof(TCommand));
                var noArgumentsCommandResult = Parse<TCommand>(serviceProvider, argumentsEnumerator);
                return noArgumentsCommandResult;
            }
        }

        public ParsingResult Parse(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
        {
            _logger.ParseForNotAlreadyKnownCommand();
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName == null)
            {
                _logger.NoCommandNameForNotAlreadyKnownCommand();
                var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new ParsingResult(null, null, CommandParsingResultCode.NoCommandNameProvided);
            }

            using (_logger.BeginParsingUsingCommandName(commandName))
            {
                var commandTypeProvider = serviceProvider.GetRequiredService<ICommandTypeProvider>();
                var commandType = commandTypeProvider.GetCommandType(commandName);
                if (commandType == null)
                {
                    _logger.NoCommandTypeFoundForNotAlreadyKnownCommand(commandName);
                    var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                    helpWriter.WriteCommandListing(_parserOptions);
                    return new ParsingResult(null, null, CommandParsingResultCode.NoCommandFound);
                }

                _logger.CommandTypeFoundForNotAlreadyKnownCommand(commandName);
                return ParseImpl(argumentsEnumerator, serviceProvider, commandType);
            }

        }

        private ParsingResult ParseImpl(IEnumerator<string> argumentsEnumerator, IServiceProvider serviceProvider, ICommandType commandType)
        {
            var commandObjectBuilder = ExtractCommandLineOptions(commandType, serviceProvider, argumentsEnumerator);
            var validation = commandObjectBuilder.Validate(serviceProvider);
            if (!validation.IsValid)
            {
                _logger.ParsedCommandIsNotValid();
                var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteHelpForCommand(_parserOptions, commandType);
                return new ParsingResult(commandObjectBuilder.Generate(), validation.ValidationErrors, CommandParsingResultCode.CommandParametersNotValid);
            }
            return new ParsingResult(commandObjectBuilder.Generate(), null, CommandParsingResultCode.Success);
        }
        private ICommandObjectBuilder ExtractCommandLineOptions(ICommandType commandType, IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
        {
            var commandObjectBuilder = commandType.CreateCommandObjectBuilder(serviceProvider, _parserOptions);
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
                    var defaultCommandOptionFinder = commandOptionFinder;
                    commandOptionFinder = (co, optionName) => co.FindOptionByShortName(optionName) ?? defaultCommandOptionFinder(co, optionName);
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
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandType.Metadata.Name, optionText));
                }

                if (!option.OptionalValue)
                {
                    value = value ?? argumentsEnumerator.GetNextCommandLineItem();
                }

                option.AssignValue(value);
            }
            return commandObjectBuilder;
        }
    }
}
