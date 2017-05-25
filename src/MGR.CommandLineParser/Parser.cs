using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.DependencyInjection;

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
            var dependencyResolverScope = DependencyResolver.Current.CreateScope();

            var parserEngine = new ParserEngine(_parserOptions);
            var result = parserEngine.Parse<TCommand>(dependencyResolverScope, args);

            return result;
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
            var dependencyResolverScope = DependencyResolver.Current.CreateScope();

            var parserEngine = new ParserEngine(_parserOptions);
            var result = parserEngine.Parse(dependencyResolverScope, args);

            return result;
        }
    }
}