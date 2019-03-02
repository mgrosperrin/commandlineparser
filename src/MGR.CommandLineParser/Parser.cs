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

        public string Logo => _parserOptions.Logo;

        public string CommandLineName => _parserOptions.CommandLineName;

        public ParsingResult Parse<TCommand>(IEnumerable<string> arguments) where TCommand : class, ICommand => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                   parserEngine.Parse<TCommand>(dependencyResolverScope, argumentsEnumerator));

        public ParsingResult Parse(IEnumerable<string> arguments) => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                parserEngine.Parse(dependencyResolverScope, argumentsEnumerator));

        public ParsingResult ParseWithDefaultCommand<TCommand>(IEnumerable<string> arguments) where TCommand : class, ICommand => ParseArguments(arguments, (parserEngine, dependencyResolverScope, argumentsEnumerator) =>
                    parserEngine.ParseWithDefaultCommand<TCommand>(dependencyResolverScope, argumentsEnumerator));

        private ParsingResult ParseArguments(IEnumerable<string> arguments, Func<ParserEngine, IServiceProvider, IEnumerator<string>, ParsingResult> callParse)
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