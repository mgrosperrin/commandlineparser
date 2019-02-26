using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MGR.CommandLineParser
{
    internal sealed class Parser : IParser
    {
        private readonly IParserOptions _parserOptions;
        private readonly IServiceProvider _serviceProvider;

        internal Parser(IParserOptions parserOptions, IServiceProvider serviceProvider)
        {
            _parserOptions = parserOptions;
            _serviceProvider = serviceProvider;
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
        public ParsingResult Parse<TCommand>(IEnumerable<string> arguments) where TCommand : class, ICommand => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                   parserEngine.Parse<TCommand>(dependencyResolverScope, argumentsEnumerator));

        /// <summary>
        ///     Parse a command line.
        /// </summary>
        /// <param name="arguments">The command line args.</param>
        /// <returns>The result of the parsing.</returns>
        public ParsingResult Parse(IEnumerable<string> arguments) => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                parserEngine.Parse(dependencyResolverScope, argumentsEnumerator));

        /// <summary>
        ///     Parse the supplied arguments for a specific command. The name of the command should not be in the arguments list.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The result of the parsing of the arguments.</returns>
        public ParsingResult ParseWithDefaultCommand<TCommand>(IEnumerable<string> arguments) where TCommand : class, ICommand => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                    parserEngine.ParseWithDefaultCommand<TCommand>(dependencyResolverScope, argumentsEnumerator));

        private ParsingResult ParseArguments<TCommand>(IEnumerable<string> arguments, Func<ParserEngine, IServiceProvider, IEnumerator<string>, ParsingResult> callParse)
            where TCommand : class, ICommand
        {
            if (arguments == null)
            {
                return new ParsingResult(null, null, CommandParsingResultCode.NoArgumentsProvided);
            }

            var loggerFactory = _serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
            var logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
            using (logger.BeginParsingArguments(Guid.NewGuid().ToString()))
            {
                logger.CreationOfParserEngine();
                var parserEngine = new ParserEngine(_parserOptions, loggerFactory);
                var argumentsEnumerator = arguments.GetArgumentsEnumerator();

                var result = callParse(parserEngine, _serviceProvider, argumentsEnumerator);
                return result;
            }
        }
    }
}