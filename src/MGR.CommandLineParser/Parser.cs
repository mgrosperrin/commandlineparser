using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents a parser.
    /// </summary>
    public sealed class Parser : IParser
    {
        private static string GetNextCommandLineItem(IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }
        /// <summary>
        /// Gets the logo used by the parser.
        /// </summary>
        public string Logo => _options.Logo;

        /// <summary>
        /// Gets the name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName => _options.CommandLineName;


        private readonly IParserOptions _options;

        internal Parser(IParserOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets the command used by the parser if called via <seealso cref="Parse{T}"/> method.
        /// </summary>
        public ICommand UniqueCommand { get; private set; }
        /// <summary>
        /// Parse a command line considering a unique command.
        /// </summary>
        /// <typeparam name="T">Used this unique type of command.</typeparam>
        /// <param name="args">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<T> Parse<T>(IEnumerable<string> args) where T : class, ICommand
        {
            if (args == null)
            {
                return new CommandResult<T>(default(T), CommandResultCode.NoArgs);
            }
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
            commandProvider.BuildCommands();
            foreach (var command in commandProvider.GetAllCommands())
            {
                if (command.GetType() == typeof (T))
                {
                    UniqueCommand = command;
                    break;
                }
            }
            CommandResult<ICommand> parsingResult = ParseImpl(args);
            if (parsingResult.Command == null)
            {
                return new CommandResult<T>(default(T), parsingResult.ReturnCode);
            }
            return new CommandResult<T>((T) parsingResult.Command, parsingResult.ReturnCode, parsingResult.ValidationResults.ToList());
        }
        /// <summary>
        /// Parse a command line.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<ICommand> Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                return new CommandResult<ICommand>(null, CommandResultCode.NoArgs);
            }
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
            commandProvider.BuildCommands();
            return ParseImpl(args);
        }

        private CommandResult<ICommand> ParseImpl(IEnumerable<string> args)
        {
            DefineCurrentHelpCommand();

            IEnumerator<string> argsEnumerator = args.GetEnumerator();
            string commandName = GetCommandName(argsEnumerator);
            if (commandName == null)
            {
                WriteHelp();
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandName);
            }
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
            commandProvider.BuildCommands();
            ICommand command = commandProvider.GetCommand(commandName);
            if (command == null)
            {
                WriteHelp();
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandFound);
            }
            var commandMetadata = command.ExtractMetadata();
            ExtractCommandLineOptions(commandMetadata, argsEnumerator);
            Tuple<bool, List<ValidationResult>> validation = Validate(command);
            if (!validation.Item1)
            {
                HelpCommand.Current.WriteHelp(commandMetadata.Command);
                return new CommandResult<ICommand>(command, CommandResultCode.CommandParameterNotValid, validation.Item2);
            }
            return new CommandResult<ICommand>(command, CommandResultCode.Ok);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IHelpCommend"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ICommandProvider"),
         SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CommandLineParser")]
        private void DefineCurrentHelpCommand()
        {
            var commandProvider = ServiceResolver.Current.ResolveService<ICommandProvider>();
            var helpCommand = commandProvider.GetHelpCommand();
            if (helpCommand == null)
            {
                throw new CommandLineParserException(string.Format(CultureInfo.CurrentCulture, "Your implementation of MGR.CommandLineParser.ICommandProvider should provide an implementation of IHelpCommend for the command's name '{0}'.",
                                                                   HelpCommand.Name));
            }
            HelpCommand.Current = helpCommand;
            HelpCommand.Current.DefineOptions(_options.AsReadOnly());
        }

        private Tuple<bool, List<ValidationResult>> Validate(ICommand command)
        {
            var validationContext = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(command, validationContext, results, true);
            if (!isValid)
            {
                var console = ServiceResolver.Current.ResolveService<IConsole>();
                console.WriteError("Command {0} : invalid arguments :", command.ExtractCommandName());
                foreach (var validation in results)
                {
                    console.WriteError(string.Format(CultureInfo.CurrentUICulture, "-{0} :", validation.ErrorMessage));
                    foreach (string memberName in validation.MemberNames)
                    {
                        console.WriteError(string.Format(CultureInfo.CurrentUICulture, "  -{0}", memberName));
                    }
                }
            }
            return Tuple.Create(isValid, results);
        }

        private void ExtractCommandLineOptions(CommandMetadata commandMetadata, IEnumerator<string> argsEnumerator)
        {
            while (true)
            {
                string argument = GetNextCommandLineItem(argsEnumerator);
                if (argument == null)
                {
                    break;
                }

                if (!(argument.StartsWith("-", StringComparison.OrdinalIgnoreCase) || argument.StartsWith("/", StringComparison.OrdinalIgnoreCase)))
                {
                    commandMetadata.Command.Arguments.Add(argument);
                    continue;
                }

                string optionText = argument.Substring(1);
                string value = null;
                int splitIndex = optionText.IndexOf(':');
                if (splitIndex > 0)
                {
                    value = optionText.Substring(splitIndex + 1);
                    optionText = optionText.Substring(0, splitIndex);
                }

                OptionMetadata option = commandMetadata.GetOption(optionText);
                if (option == null)
                {
                    throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "There is no option '{1}' for the command '{0}'.", commandMetadata.Name, optionText));
                }

                if (option.OptionType == typeof (bool))
                {
                    value = value ?? "true";
                }
                else
                {
                    value = value ?? GetNextCommandLineItem(argsEnumerator);
                }

                if (value == null)
                {
                    throw new CommandLineParserException(string.Format(CultureInfo.CurrentUICulture, "You should specified a value for the option '{1}' of the command '{0}'.", commandMetadata.Name, optionText));
                }

                option.AssignValue(value);
            }
        }

        private static void WriteHelp()
        {
            HelpCommand.Current.Execute();
        }

        private string GetCommandName(IEnumerator<string> argsEnumerator)
        {
            if (UniqueCommand == null)
            {
                return GetNextCommandLineItem(argsEnumerator);
            }
            return UniqueCommand.ExtractCommandName();
        }
    }
}