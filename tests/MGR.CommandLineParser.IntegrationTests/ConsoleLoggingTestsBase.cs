using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests
{
    public abstract class ConsoleLoggingTestsBase
    {
        private static readonly ServiceProvider _serviceProvider;
#pragma warning disable S3963 // "static" fields should be initialized inline
        static ConsoleLoggingTestsBase()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IConsole, FakeConsole>();
            serviceCollection.AddCommandLineParser().AddClassBasedCommands();
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline
        protected readonly FakeConsole _console;
        protected readonly IServiceProvider _scopedServiceProvider;
        protected ConsoleLoggingTestsBase()
        {
            _scopedServiceProvider = _serviceProvider.CreateScope().ServiceProvider;
            _console = (FakeConsole)_scopedServiceProvider.GetRequiredService<IConsole>();
        }

        protected ParsingResult CallParse(IEnumerable<string> args)
        {
            var parserBuilder = new ParserBuilder();
            var parsingResult = CallParse(parserBuilder, args);

            return parsingResult;
        }

        protected ParsingResult CallParse(ParserBuilder parserBuilder, IEnumerable<string> args)
        {
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.Parse(args, _scopedServiceProvider);

            return parsingResult;
        }
        protected ParsingResult CallParse<TCommand>(IEnumerable<string> args)
        where TCommand: class, ICommand
        {
            var parserBuilder = new ParserBuilder();
            var parsingResult = CallParse<TCommand>(parserBuilder, args);

            return parsingResult;
        }

        protected ParsingResult CallParse<TCommand>(ParserBuilder parserBuilder, IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.Parse<TCommand>(args, _scopedServiceProvider);

            return parsingResult;
        }
        protected ParsingResult CallParseWithDefaultCommand<TCommand>(IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parserBuilder = new ParserBuilder();
            var parsingResult = CallParseWithDefaultCommand<TCommand>(parserBuilder, args);

            return parsingResult;
        }

        protected ParsingResult CallParseWithDefaultCommand<TCommand>(ParserBuilder parserBuilder, IEnumerable<string> args)
            where TCommand : class, ICommand
        {
            var parser = parserBuilder.BuildParser();
            var parsingResult = parser.ParseWithDefaultCommand< TCommand>(args, _scopedServiceProvider);

            return parsingResult;
        }

        protected void AssertNoMessage()
        {
            var messages = _console.Messages;
            Assert.Empty(messages);
        }

        protected void AssertOneMessageLoggedToConsole<TMessage>(string expectedMessage)
        where TMessage : FakeConsole.Message
        {
            var messages = _console.Messages;
            Assert.Single(messages);
            Assert.IsType<TMessage>(messages[0]);
            Assert.Equal(expectedMessage, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }
    }
}