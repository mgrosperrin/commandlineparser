using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Properties;
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

        public CommandResult<TCommand> Parse<TCommand>(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator) where TCommand : class, ICommand
        {
            using (_logger.BeginParsingForSpecificCommandType(typeof(TCommand)))
            {
                var commandTypeProvider = serviceProvider.GetRequiredService<ICommandTypeProvider>();
                var commandType = commandTypeProvider.GetCommandType<TCommand>();
                var parsingResult = ParseImpl(argumentsEnumerator, serviceProvider, commandType);
                if (parsingResult.Command == null)
                {
                    _logger.NoCommandFoundAfterSpecificParsing();
                    return new CommandResult<TCommand>(default(TCommand), parsingResult.ParsingResultCode);
                }

                _logger.CommandFoundAfterSpecificParsing(parsingResult.Command.GetType(), parsingResult.ParsingResultCode, parsingResult.ValidationResults);
                return new CommandResult<TCommand>((TCommand) parsingResult.Command, parsingResult.ParsingResultCode,
                    parsingResult.ValidationResults.ToList());
            }
        }

        public CommandResult<ICommand> ParseWithDefaultCommand<TCommand>(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
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
                    return new CommandResult<ICommand>(withArgumentsCommandResult.Command, withArgumentsCommandResult.ParsingResultCode,
                        withArgumentsCommandResult.ValidationResults.ToList());

                }

                _logger.NoArgumentProvidedWithDefaultCommandType(typeof(TCommand));
                var noArgumentsCommandResult = Parse<TCommand>(serviceProvider, argumentsEnumerator);
                return new CommandResult<ICommand>(noArgumentsCommandResult.Command, noArgumentsCommandResult.ParsingResultCode,
                    noArgumentsCommandResult.ValidationResults.ToList());
            }
        }

        public CommandResult<ICommand> Parse(IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
        {
            _logger.ParseForNotAlreadyKnownCommand();
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName == null)
            {
                _logger.NoCommandNameForNotAlreadyKnownCommand();
                var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new CommandResult<ICommand>(null, CommandParsingResultCode.NoCommandNameProvided);
            }

            using (_logger.BeginParsingUsingCommandName(commandName))
            {
                _logger.LogInformation("The command name is {commandName}", commandName);
                var commandTypeProvider = serviceProvider.GetRequiredService<ICommandTypeProvider>();
                var commandType = commandTypeProvider.GetCommandType(commandName);
                if (commandType == null)
                {
                    _logger.NoCommandTypeFoundForNotAlreadyKnownCommand(commandName);
                    var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                    helpWriter.WriteCommandListing(_parserOptions);
                    return new CommandResult<ICommand>(null, CommandParsingResultCode.NoCommandFound);
                }

                _logger.CommandTypeFoundForNotAlreadyKnownCommand(commandName);
                return ParseImpl(argumentsEnumerator, serviceProvider, commandType);
            }

        }

        private CommandResult<ICommand> ParseImpl(IEnumerator<string> argumentsEnumerator, IServiceProvider serviceProvider, ICommandType commandType)
        {
            var command = ExtractCommandLineOptions(commandType, serviceProvider, argumentsEnumerator);
            var validation = Validate(command, serviceProvider, commandType.Metadata.Name);
            if (!validation.Item1)
            {
                _logger.ParsedCommandIsNotValid();
                var helpWriter = serviceProvider.GetRequiredService<IHelpWriter>();
                helpWriter.WriteHelpForCommand(_parserOptions, commandType);
                return new CommandResult<ICommand>(command, CommandParsingResultCode.CommandParametersNotValid, validation.Item2);
            }
            return new CommandResult<ICommand>(command, CommandParsingResultCode.Success);
        }
        private ICommand ExtractCommandLineOptions(ICommandType commandType, IServiceProvider serviceProvider, IEnumerator<string> argumentsEnumerator)
        {
            var command = commandType.CreateCommand(serviceProvider, _parserOptions);
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
                    command.Arguments.Add(argument);
                    continue;
                }

                var starterLength = 2;
                Func<ICommandType, string, ICommandOption> commandOptionFinder = (ct, optionName) => ct.FindOption(optionName);
                if (!argument.StartsWith(Constants.LongNameOptionStarter))
                {
                    starterLength = Constants.ShortNameOptionStarter.Length;
                    var defaultCommandOptionFinder = commandOptionFinder;
                    commandOptionFinder = (ct, optionName) => ct.FindOptionByShortName(optionName) ?? defaultCommandOptionFinder(ct, optionName);
                }
                var optionText = argument.Substring(starterLength);
                string value = null;
                var splitIndex = optionText.IndexOf(Constants.OptionSplitter);
                if (splitIndex > 0)
                {
                    value = optionText.Substring(splitIndex + 1);
                    optionText = optionText.Substring(0, splitIndex);
                }

                var option = commandOptionFinder(commandType, optionText);
                if (option == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandType.Metadata.Name, optionText));
                }

                if (!option.OptionalValue)
                {
                    value = value ?? argumentsEnumerator.GetNextCommandLineItem();
                }

                option.AssignValue(value, command);
            }
            return command;
        }

        private static Tuple<bool, List<ValidationResult>> Validate(ICommand command, IServiceProvider serviceProvider, string commandName)
        {
            var validationContext = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(command, validationContext, results, true);
            if (!isValid)
            {
                var console = serviceProvider.GetRequiredService<IConsole>();
                console.WriteError(Strings.Parser_CommandInvalidArgumentsFormat, commandName);
                foreach (var validation in results)
                {
                    console.WriteError(string.Format(CultureInfo.CurrentUICulture, "-{0} :", validation.ErrorMessage));
                    foreach (var memberName in validation.MemberNames)
                    {
                        console.WriteError(string.Format(CultureInfo.CurrentUICulture, "  -{0}", memberName));
                    }
                }
            }
            return Tuple.Create(isValid, results);
        }
    }
}
