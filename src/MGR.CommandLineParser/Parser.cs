using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Diagnostics;
using MGR.CommandLineParser.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MGR.CommandLineParser
{
    internal sealed class Parser : IParser
    {
        private readonly IParserOptions _parserOptions;

        internal Parser(IParserOptions parserOptions)
        {
            _parserOptions = parserOptions;
        }

        public string Logo => _parserOptions.Logo;

        public string CommandLineName => _parserOptions.CommandLineName;

        public ParsingResult Parse<TCommand>(IEnumerable<string> arguments, IServiceProvider serviceProvider) where TCommand : class, ICommand => ParseArguments(arguments, serviceProvider, (parserEngine, argumentsEnumerator) =>
                   parserEngine.Parse<TCommand>(argumentsEnumerator));

        public ParsingResult Parse(IEnumerable<string> arguments, IServiceProvider serviceProvider) => ParseArguments(arguments, serviceProvider, (parserEngine, argumentsEnumerator) =>
                parserEngine.Parse(argumentsEnumerator));

        public ParsingResult ParseWithDefaultCommand<TCommand>(IEnumerable<string> arguments, IServiceProvider serviceProvider) where TCommand : class, ICommand => ParseArguments(arguments, serviceProvider, (parserEngine, argumentsEnumerator) =>
                    parserEngine.ParseWithDefaultCommand<TCommand>(argumentsEnumerator));

        private ParsingResult ParseArguments(IEnumerable<string> arguments, IServiceProvider serviceProvider, Func<ParserEngine, IEnumerator<string>, ParsingResult> callParse)
        {
            if (arguments == null)
            {
                return new ParsingResult(null, null, CommandParsingResultCode.NoArgumentsProvided);
            }

            var parserOptionsAccessor = serviceProvider.GetService<IParserOptionsAccessor>();
            parserOptionsAccessor.Current = _parserOptions;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
            var logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
            using (logger.BeginParsingArguments(Guid.NewGuid().ToString()))
            {
                logger.CreationOfParserEngine();
                var parserEngine = new ParserEngine(serviceProvider, loggerFactory);
                var argumentsEnumerator = arguments.GetArgumentsEnumerator();

                var result = callParse(parserEngine, argumentsEnumerator);
                return result;
            }
        }
    }
}