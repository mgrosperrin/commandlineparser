using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Properties;

namespace MGR.CommandLineParser
{
    internal sealed class Parser : IParser
    {
        private readonly IParserOptions _parserOptions;

        internal Parser(IParserOptions parserOptions)
        {
            _parserOptions = parserOptions;
        }

        /// <summary>
        ///     Gets the logo used by the parser.
        /// </summary>
        public string Logo => _parserOptions.Logo;

        /// <summary>
        ///     Gets the name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName => _parserOptions.CommandLineName;

        /// <summary>
        ///     Parse a command line considering a unique command.
        /// </summary>
        /// <typeparam name="TCommand">Used this unique type of command.</typeparam>
        /// <param name="args">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<TCommand> Parse<TCommand>(IEnumerable<string> args) where TCommand : class, ICommand
        {
            if (args == null)
            {
                return new CommandResult<TCommand>(default(TCommand), CommandResultCode.NoArgs);
            }
            var currentDependencyResolver = DependencyResolver.Current.CreateScope();
            var commandTypeProvider = currentDependencyResolver.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType<TCommand>();
            var argsEnumerator = args.GetArgumentEnumerator();
            var parsingResult = ParseImpl(argsEnumerator, currentDependencyResolver, commandType);
            if (parsingResult.Command == null)
            {
                return new CommandResult<TCommand>(default(TCommand), parsingResult.ReturnCode);
            }
            return new CommandResult<TCommand>((TCommand)parsingResult.Command, parsingResult.ReturnCode, parsingResult.ValidationResults.ToList());
        }

        /// <summary>
        ///     Parse a command line.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<ICommand> Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                return new CommandResult<ICommand>(null, CommandResultCode.NoArgs);
            }
            var currentDependencyResolver = DependencyResolver.Current.CreateScope();
            var argsEnumerator = args.GetArgumentEnumerator();
            var commandName = GetNextCommandLineItem(argsEnumerator);
            if (commandName == null)
            {
                var helpWriter = currentDependencyResolver.ResolveDependency<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandName);
            }
            var commandTypeProvider = currentDependencyResolver.ResolveDependency<ICommandTypeProvider>();
            var commandType = commandTypeProvider.GetCommandType(commandName);
            if (commandType == null)
            {
                var helpWriter = currentDependencyResolver.ResolveDependency<IHelpWriter>();
                helpWriter.WriteCommandListing(_parserOptions);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandFound);
            }
            return ParseImpl(argsEnumerator, currentDependencyResolver, commandType);
        }

        private static string GetNextCommandLineItem(IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }

        private CommandResult<ICommand> ParseImpl(IEnumerator<string> argsEnumerator, IDependencyResolverScope dependencyResolver, CommandType commandType)
        {
            var command = ExtractCommandLineOptions(commandType, dependencyResolver, argsEnumerator);
            var validation = Validate(command, dependencyResolver, commandType.Metadata.Name);
            if (!validation.Item1)
            {
                var helpWriter = dependencyResolver.ResolveDependency<IHelpWriter>();
                helpWriter.WriteHelpForCommand(_parserOptions, commandType);
                return new CommandResult<ICommand>(command, CommandResultCode.CommandParameterNotValid, validation.Item2);
            }
            return new CommandResult<ICommand>(command, CommandResultCode.Ok);
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

        private ICommand ExtractCommandLineOptions(CommandType commandType, IDependencyResolverScope dependencyResolver, IEnumerator<string> argsEnumerator)
        {
            var command = commandType.CreateCommand(dependencyResolver, _parserOptions);
            var alwaysPutInArgumentList = false;
            while (true)
            {
                var argument = GetNextCommandLineItem(argsEnumerator);
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
                    value = value ?? GetNextCommandLineItem(argsEnumerator);
                }

                if (value == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionValueRequired(commandType.Metadata.Name, optionText));
                }

                option.AssignValue(value, command);
            }
            return command;
        }
    }
}