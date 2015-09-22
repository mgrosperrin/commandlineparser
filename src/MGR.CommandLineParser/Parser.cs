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
            //SetUniqueCommand<TCommand>();
            var commandName = typeof (TCommand).ExtractCommandMetadataTemplate().Name;
            var parsingResult = ParseImpl(args, commandName);
            if (parsingResult.Command == null)
            {
                return new CommandResult<TCommand>(default(TCommand), parsingResult.ReturnCode);
            }
            return new CommandResult<TCommand>((TCommand) parsingResult.Command, parsingResult.ReturnCode, parsingResult.ValidationResults.ToList());
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
            return ParseImpl(args, string.Empty);
        }

        private static string GetNextCommandLineItem(IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }

        private CommandResult<ICommand> ParseImpl(IEnumerable<string> args, string commandName)
        {
            var argsEnumerator = args.GetEnumerator();
            var commandResolver = ServiceResolver.Current.ResolveService<CommandResolver>();
            var console = ServiceResolver.Current.ResolveService<IConsole>();
            if (string.IsNullOrEmpty(commandName))
            {
                commandName = GetCommandName(argsEnumerator);
            }
            if (commandName == null)
            {
                WriteHelp(commandResolver, console);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandName);
            }
            var command = commandResolver.GetCommand(commandName, _parserOptions, console);
            if (command == null)
            {
                WriteHelp(commandResolver, console);
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandFound);
            }
            var commandMetadata = command.ExtractMetadata();
            ExtractCommandLineOptions(commandMetadata, argsEnumerator);
            var validation = Validate(command);
            if (!validation.Item1)
            {
                commandResolver.GetHelpCommand(_parserOptions, console).WriteHelp(commandMetadata.Command);
                return new CommandResult<ICommand>(command, CommandResultCode.CommandParameterNotValid, validation.Item2);
            }
            return new CommandResult<ICommand>(command, CommandResultCode.Ok);
        }

        private static Tuple<bool, List<ValidationResult>> Validate(ICommand command)
        {
            var validationContext = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(command, validationContext, results, true);
            if (!isValid)
            {
                var console = ServiceResolver.Current.ResolveService<IConsole>();
                console.WriteError(Strings.Parser_CommandInvalidArgumentsFormat, command.ExtractCommandName());
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

        private static void ExtractCommandLineOptions(CommandMetadata commandMetadata, IEnumerator<string> argsEnumerator)
        {
            while (true)
            {
                var argument = GetNextCommandLineItem(argsEnumerator);
                if (argument == null)
                {
                    break;
                }

                if (!(argument.StartsWith(StringComparison.OrdinalIgnoreCase, Constants.ArgumentStarter)))
                {
                    commandMetadata.Command.Arguments.Add(argument);
                    continue;
                }

                var optionText = argument.Substring(1);
                string value = null;
                var splitIndex = optionText.IndexOf(Constants.ArgumentSplitter);
                if (splitIndex > 0)
                {
                    value = optionText.Substring(splitIndex + 1);
                    optionText = optionText.Substring(0, splitIndex);
                }

                var option = commandMetadata.GetOption(optionText);
                if (option == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionNotFoundForCommand(commandMetadata.Name, optionText));
                }

                if (option.OptionType == typeof (bool))
                {
                    value = value ?? bool.TrueString;
                }
                else
                {
                    value = value ?? GetNextCommandLineItem(argsEnumerator);
                }

                if (value == null)
                {
                    throw new CommandLineParserException(Constants.ExceptionMessages.FormatParserOptionValueRequired(commandMetadata.Name, optionText));
                }

                option.AssignValue(value);
            }
        }

        private void WriteHelp(CommandResolver commandResolver, IConsole console)
        {
            commandResolver.GetHelpCommand(_parserOptions, console).Execute();
        }

        private static string GetCommandName(IEnumerator<string> argsEnumerator) => GetNextCommandLineItem(argsEnumerator);
    }
}