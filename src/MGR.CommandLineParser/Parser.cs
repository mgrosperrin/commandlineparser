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
    public sealed class Parser
    {
        #region Public static methods

        /// <summary>
        /// Creates a new instance of <see cref="Parser"/> with the default options.
        /// </summary>
        /// <returns>A new instance of <see cref="Parser"/>.</returns>
        public static Parser Create()
        {
            return Create(new ParserOptions());
        }

        /// <summary>
        /// Creates a new instance of <see cref="Parser"/> with the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="ParserOptions"/> to use to initialize the parser.</param>
        /// <returns>A new instance of <see cref="Parser"/>.</returns>
        public static Parser Create(ParserOptions options)
        {
            return new Parser(options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Parser"/> with a custom <see cref="IConsole"/>.
        /// </summary>
        /// <param name="console">The custom <see cref="IConsole"/>.</param>
        /// <returns>A new instance of <see cref="Parser"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="console"/> is null.</exception>
        public static Parser WithCustomConsole(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            return Create(new ParserOptions {Console = console});
        }

        /// <summary>
        /// Creates a new instance of <see cref="Parser"/> with a custom <see cref="ICommandProvider"/>.
        /// </summary>
        /// <param name="commandProvider">The custom <see cref="ICommandProvider"/>.</param>
        /// <returns>A new instance of <see cref="Parser"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandProvider"/> is null.</exception>
        public static Parser WithCustomCommandProvider(ICommandProvider commandProvider)
        {
            if (commandProvider == null)
            {
                throw new ArgumentNullException("commandProvider");
            }
            return Create(new ParserOptions {CommandProvider = commandProvider});
        }

        #endregion

        private static string GetNextCommandLineItem(IEnumerator<string> argsEnumerator)
        {
            if (argsEnumerator == null || !argsEnumerator.MoveNext())
            {
                return null;
            }
            return argsEnumerator.Current;
        }

        #region Parser options customization

        /// <summary>
        /// Redefines a new custom <see cref="IConsole"/>.
        /// </summary>
        /// <param name="console">The new <see cref="IConsole"/>.</param>
        /// <returns>The <see cref="Parser"/> with the new <see cref="IConsole"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="console"/> is null.</exception>
        public Parser DefineConsole(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _options.Console = console;
            return this;
        }

        /// <summary>
        /// Gets the <see cref="IConsole"/> used by the parser.
        /// </summary>
        public IConsole Console
        {
            get { return _options.Console; }
        }

        /// <summary>
        /// Redefines a new custom <see cref="ICommandProvider"/>.
        /// </summary>
        /// <param name="commandProvider">The new <see cref="ICommandProvider"/>.</param>
        /// <returns>The <see cref="Parser"/> with the new <see cref="ICommandProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandProvider"/> is null.</exception>
        public Parser DefineCommandProvider(ICommandProvider commandProvider)
        {
            if (commandProvider == null)
            {
                throw new ArgumentNullException("commandProvider");
            }
            _options.CommandProvider = commandProvider;
            return this;
        }

        /// <summary>
        /// Gets the <see cref="ICommandProvider"/> used by the parser.
        /// </summary>
        public ICommandProvider CommandProvider
        {
            get { return _options.CommandProvider; }
        }

        /// <summary>
        /// Redefines a new custom logo.
        /// </summary>
        /// <param name="logo">The new logo.</param>
        /// <returns>The <see cref="Parser"/> with the new logo.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logo"/> is null.</exception>
        public Parser DefineLogo(string logo)
        {
            if (logo == null)
            {
                throw new ArgumentNullException("logo");
            }
            _options.Logo = logo;
            return this;
        }
        /// <summary>
        /// Gets the logo used by the parser.
        /// </summary>
        public string Logo
        {
            get { return _options.Logo; }
        }

        /// <summary>
        /// Redefines a new custom name of the executable to run.
        /// </summary>
        /// <param name="commandLineName">The new name of the executable to run.</param>
        /// <returns>The <see cref="Parser"/> with the new <see cref="ICommandProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandLineName"/> is null.</exception>
        public Parser DefineCommandLineName(string commandLineName)
        {
            if (commandLineName == null)
            {
                throw new ArgumentNullException("commandLineName");
            }
            _options.CommandLineName = commandLineName;
            return this;
        }

        /// <summary>
        /// Gets the name of the executable to run used in the help by the parser.
        /// </summary>
        public string CommandLineName
        {
            get { return _options.CommandLineName; }
        }

        /// <summary>
        /// Add a new custom <see cref="IConverter"/> if no <see cref="IConverter"/> is defined with the same <seealso cref="IConverter.TargetType"/>.
        /// </summary>
        /// <param name="converter">The new <see cref="IConverter"/>.</param>
        /// <returns>The <see cref="Parser"/> with the new <see cref="IConverter"/> added to the <seealso cref="Converters"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="converter"/> is null.</exception>
        public Parser DefineConverter(IConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            DefineConverter(converter, true /* Default Value */);
            return this;
        }

        /// <summary>
        /// Add a new custom <see cref="IConverter"/> and possibly overwrites if an <see cref="IConverter"/> is defined with the same <seealso cref="IConverter.TargetType"/>.
        /// </summary>
        /// <param name="converter">The new <see cref="IConverter"/>.</param>
        /// <param name="overwrite">true to overwrites existing <see cref="IConverter"/>.</param>
        /// <returns>The <see cref="Parser"/> with the new <see cref="ICommandProvider"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="converter"/> is null.</exception>
        public Parser DefineConverter(IConverter converter, bool overwrite)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            _options.DefineConverter(converter, overwrite);
            return this;
        }

        /// <summary>
        /// Gets the collection of <see cref="IConverter"/> used by the parser.
        /// </summary>
        public IEnumerable<IConverter> Converters
        {
            get { return _options.Converters.AsEnumerable(); }
        }

        #endregion

        private readonly ParserOptions _options;

        private Parser(ParserOptions options)
        {
            _options = options.ConsolidateOptions();
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
            _options.CommandProvider.BuildCommands();
            foreach (var command in CommandProvider.AllCommands)
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
            _options.CommandProvider.BuildCommands();
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
            ICommand command = CommandProvider.GetCommand(commandName);
            if (command == null)
            {
                WriteHelp();
                return new CommandResult<ICommand>(null, CommandResultCode.NoCommandFound);
            }
            var commandBase = command as CommandBase;
            if (commandBase != null)
            {
                commandBase.DefineConsole(_options.Console);
            }
            var commandMetadata = command.ExtractMetadata(_options);
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
            var helpCommand = _options.CommandProvider.GetHelpCommand();
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
                Console.WriteError("Command {0} : invalid arguments :", command.ExtractCommandName());
                foreach (var validation in results)
                {
                    Console.WriteError(string.Format(CultureInfo.CurrentUICulture, "-{0} :", validation.ErrorMessage));
                    foreach (string memberName in validation.MemberNames)
                    {
                        Console.WriteError(string.Format(CultureInfo.CurrentUICulture, "  -{0}", memberName));
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

                option.AssignValue(value, _options);
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