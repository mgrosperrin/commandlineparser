using System;
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
        /// <param name="arguments">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<TCommand> Parse<TCommand>(IEnumerable<string> arguments) where TCommand : class, ICommand
        {
            return ParseArguments(arguments,
                (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                    parserEngine.Parse<TCommand>(dependencyResolverScope, argumentsEnumerator));
        }

        /// <summary>
        ///     Parse a command line.
        /// </summary>
        /// <param name="arguments">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public CommandResult<ICommand> Parse(IEnumerable<string> arguments)
        {
            return ParseArguments(arguments,
                (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                parserEngine.Parse(dependencyResolverScope, argumentsEnumerator));
        }

        private CommandResult<TCommand> ParseArguments<TCommand>(IEnumerable<string> arguments, Func<ParserEngine, IDependencyResolverScope, IEnumerator<string>, CommandResult<TCommand>> callParse)
            where TCommand : class, ICommand
        {
            if (arguments == null)
            {
                return new CommandResult<TCommand>(null, CommandResultCode.NoArgs);
            }
            var dependencyResolverScope = DependencyResolver.Current.CreateScope();

            var parserEngine = new ParserEngine(_parserOptions);
            var argumentsEnumerator = arguments.GetArgumentsEnumerator();

            var result = callParse(parserEngine, dependencyResolverScope, argumentsEnumerator);

            return result;
        }
    }
}