using System;
using JetBrains.Annotations;
using MGR.CommandLineParser.Command;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents the constructor of a parser.
    /// </summary>
    [PublicAPI]
    public sealed class ParserBuilder
    {
        private readonly CommandLineParserBuilder _commandLineParserBuilder;

        /// <summary>
        /// Creates a new <see cref="ParserBuilder"/>.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        public ParserBuilder([NotNull] ParserOptions parserOptions)
        : this(parserOptions, new ServiceCollection())
        {
        }

        /// <summary>
        /// Creates a new <see cref="ParserBuilder"/>.
        /// </summary>
        /// <param name="parserOptions">The options of the parser.</param>
        /// <param name="services">The services to uses.</param>
        public ParserBuilder([NotNull] ParserOptions parserOptions, [NotNull] IServiceCollection services)
        {
            _commandLineParserBuilder = services.AddCommandLineParser(options =>
            {
                options.Logo = parserOptions.Logo;
                options.CommandLineName = parserOptions.CommandLineName;
            }).AddCommands<HelpCommand>();
        }

        /// <summary>
        /// Add the commands to the <see cref="ParserBuilder"/>.
        /// </summary>
        /// <param name="configureCommands">The action to add the commands.</param>
        /// <returns>This <see cref="ParserBuilder"/> configured with the commands.</returns>
        public ParserBuilder AddCommands(Action<CommandLineParserBuilder> configureCommands)
        {
            configureCommands(_commandLineParserBuilder);
            return this;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="Parser" /> with the default options.
        /// </summary>
        /// <returns>A new instance of <see cref="Parser" />.</returns>
        public IParser BuildParser()
        {
            var serviceProvider = _commandLineParserBuilder.Services.BuildServiceProvider();
            var serviceProviderScope = serviceProvider.CreateScope();
            var parser = serviceProviderScope.ServiceProvider.GetService<IParser>();
            return parser;
        }
    }
}