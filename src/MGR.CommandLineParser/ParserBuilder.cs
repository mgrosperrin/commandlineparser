using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents the constructor of a parser.
    /// </summary>
    [PublicAPI]
    public sealed class ParserBuilder
    {
        private readonly ParserBuilderOptions _parserBuilderOptions = ParserBuilderOptions.Default;
        private static readonly Lazy<ServiceProvider> ServiceProviderLazy = new Lazy<ServiceProvider>(
            CreateRootServiceProvider);

        private static ServiceProvider CreateRootServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandLineParser();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="Parser" /> with the default options.
        /// </summary>
        /// <returns>A new instance of <see cref="Parser" />.</returns>
        public IParser BuildParser() => BuildParser(ServiceProviderLazy.Value.CreateScope().ServiceProvider);
        /// <summary>
        ///     Creates a new instance of <see cref="Parser" /> with the default options.
        /// </summary>
        /// <param name="serviceProvider">A custom <see cref="ServiceProvider"/>.</param>
        /// <returns>A new instance of <see cref="Parser" />.</returns>
        public IParser BuildParser(IServiceProvider serviceProvider)
        {
            var parserOptions = _parserBuilderOptions.ToParserOptions();
            var parser = new Parser(parserOptions, serviceProvider);
            return parser;
        }

        /// <summary>
        ///     Changes the logo to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="logo">The custom logo</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#")]
        public ParserBuilder Logo(string logo)
        {
            _parserBuilderOptions.Logo = logo;
            return this;
        }

        /// <summary>
        ///     Changes the command line name to use when creating the <see cref="Parser" />.
        /// </summary>
        /// <param name="commandLineName">The custom command line name</param>
        /// <returns>This instance of <see cref="ParserBuilder" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#")]
        public ParserBuilder CommandLineName(string commandLineName)
        {
            _parserBuilderOptions.CommandLineName = commandLineName;
            return this;
        }
    }
}