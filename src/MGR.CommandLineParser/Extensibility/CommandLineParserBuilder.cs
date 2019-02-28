using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Extensibility
{
    /// <summary>
    ///
    /// </summary>
    public sealed class CommandLineParserBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public CommandLineParserBuilder(IServiceCollection services)
        {
            Services = services;
        }
        /// <summary>
        ///
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
