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

        public ParsingResult Parse<TCommand>(IEnumerable<string> args, IServiceProvider serviceProvider) where TCommand : class, ICommand => ParseArguments(args, serviceProvider, (parserEngine, arguments) =>
                   parserEngine.Parse<TCommand>(arguments));

        public ParsingResult Parse(IEnumerable<string> args, IServiceProvider serviceProvider) => ParseArguments(args, serviceProvider, (parserEngine, arguments) =>
                parserEngine.Parse(arguments));

        public ParsingResult ParseWithDefaultCommand<TCommand>(IEnumerable<string> args, IServiceProvider serviceProvider) where TCommand : class, ICommand => ParseArguments(args, serviceProvider, (parserEngine, arguments) =>
                    parserEngine.ParseWithDefaultCommand<TCommand>(arguments));

        private ParsingResult ParseArguments(IEnumerable<string> args, IServiceProvider serviceProvider, Func<ParserEngine, Arguments, ParsingResult> callParse)
        {
            if (args == null)
            {
                return new ParsingResult(null, null, CommandParsingResultCode.NoArgumentsProvided);
            }
            var arguments = new Arguments(args);
            var parserOptionsAccessor = serviceProvider.GetService<IParserOptionsAccessor>();
            parserOptionsAccessor.Current = _parserOptions;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
            var logger = loggerFactory.CreateLogger<LoggerCategory.Parser>();
            using (logger.BeginParsingArguments(Guid.NewGuid().ToString()))
            {
                logger.CreationOfParserEngine();
                var parserEngine = new ParserEngine(serviceProvider, loggerFactory);

                var result = callParse(parserEngine, arguments);
                return result;
            }
        }
    }
}