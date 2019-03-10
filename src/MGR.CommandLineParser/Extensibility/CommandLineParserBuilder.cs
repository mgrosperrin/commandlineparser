﻿using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    /// A class to add configuration to the command line parser.
    /// </summary>
    public sealed class CommandLineParserBuilder
    {
        internal CommandLineParserBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> being used to configure the command line parser.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
