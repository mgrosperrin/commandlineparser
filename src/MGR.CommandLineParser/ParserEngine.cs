using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.DependencyInjection;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    internal class ParserEngine
    {
        private readonly IParserOptions _parserOptions;

        public ParserEngine(IParserOptions parserOptions)
        {
            _parserOptions = parserOptions;
        }

        public CommandResult<TCommand> Parse<TCommand>(IDependencyResolverScope dependencyResolverScope, IEnumerator<string> argumentsEnumerator) where TCommand : class, ICommand
        {
            var commandTypeProvider = dependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType<TCommand>();
            var parsingResult = ParseImpl(argumentsEnumerator, dependencyResolverScope, commandType);
            if (parsingResult.Command == null)
            {
                return new CommandResult<TCommand>(default(TCommand), parsingResult.ReturnCode);
            }
            return new CommandResult<TCommand>((TCommand)parsingResult.Command, parsingResult.ReturnCode, parsingResult.ValidationResults.ToList());
        }

        public CommandResult<ICommand> ParseWithDefaultCommand<TCommand>(IDependencyResolverScope dependencyResolverScope, IEnumerator<string> argumentsEnumerator)
            where TCommand : class, ICommand
        {
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName == null)
            {
                var noArgumentsCommandResult = Parse<TCommand>(dependencyResolverScope, argumentsEnumerator);
                return new CommandResult<ICommand>(noArgumentsCommandResult.Command, noArgumentsCommandResult.ReturnCode, noArgumentsCommandResult.ValidationResults.ToList());
            }
            var commandTypeProvider = dependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType(commandName);
            if (commandType == null)
            {
                var noArgumentsCommandResult = Parse<TCommand>(dependencyResolverScope, argumentsEnumerator.PrefixWith(commandName));
                return new CommandResult<ICommand>(noArgumentsCommandResult.Command, noArgumentsCommandResult.ReturnCode, noArgumentsCommandResult.ValidationResults.ToList());
            }
            return ParseImpl(argumentsEnumerator, dependencyResolverScope, commandType);

        }

        public CommandResult<ICommand> Parse(IDependencyResolverScope dependencyResolverScope, IEnumerator<string> argumentsEnumerator)
        {
            var commandName = argumentsEnumerator.GetNextCommandLineItem();
            if (commandName == null)
            {
                var helpWriter = dependencyResolverScope.ResolveDependency<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandName);
            }
            var commandTypeProvider = dependencyResolverScope.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType(commandName);
            if (commandType == null)
            {
                var helpWriter = dependencyResolverScope.ResolveDependency<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandFound);
            }
            return ParseImpl(argumentsEnumerator, dependencyResolverScope, commandType);

        }

        private CommandResult<ICommand> ParseImpl(IEnumerator<string> argumentsEnumerator, IDependencyResolverScope dependencyResolver, ICommandType commandType)
        {
            var command = ExtractCommandLineOptions(commandType, dependencyResolver, argumentsEnumerator);
            var validation = Validate(command, dependencyResolver, commandType.Metadata.Name);
            if (!validation.Item1)
            {
                var helpWriter = dependencyResolver.ResolveDependency<IHelpWriter>();
                helpWriter.WriteHelpForCommand(_parserOptions, commandType);
                return new CommandResult<ICommand>(command, CommandResultCode.CommandParameterNotValid, validation.Item2);
            }
            return new CommandResult<ICommand>(command, CommandResultCode.Ok);
        }
        private ICommand ExtractCommandLineOptions(ICommandType commandType, IDependencyResolverScope dependencyResolver, IEnumerator<string> argumentsEnumerator)
        {
            var command = commandType.CreateCommand(dependencyResolver, _parserOptions);
            var alwaysPutInArgumentList = false;
            while (true)
            {
                var argument = argumentsEnumerator.GetNextCommandLineItem();
                if (argument == null)
                {
                    break;
                }
                if (argument.Equals(Constants.DoubleDash))
                {
                    alwaysPutInArgumentList = true;
                    continue;
                }

                if (alwaysPutInArgumentList || !argument.StartsWith(StringComparison.OrdinalIgnoreCase, Constants.OptionStarter))
                {
                    command.Arguments.Add(argument);
                    continue;
                }

                var optionText = argument.Substring(1);
                string value = null;
                var splitIndex = optionText.IndexOf(Constants.OptionSplitter);
                if (splitIndex > 0)
                {
                    value = optionText.Substring(splitIndex + 1);
                    optionText = optionText.Substring(0, splitIndex);
                }

                var option = commandType.FindOption(optionText);
                if (option == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandType.Metadata.Name, optionText));
                }

                if (option.OptionType == typeof(bool))
                {
                    value = value ?? bool.TrueString;
                }
                else
                {
                    value = value ?? argumentsEnumerator.GetNextCommandLineItem();
                }

                if (value == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionValueRequired(commandType.Metadata.Name, optionText));
                }

                option.AssignValue(value, command);
            }
            return command;
        }

        private static Tuple<bool, List<ValidationResult>> Validate(ICommand command, IDependencyResolverScope dependencyResolver, string commandName)
        {
            var validationContext = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(command, validationContext, results, true);
            if (!isValid)
            {
                var console = dependencyResolver.ResolveDependency<IConsole>();
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
