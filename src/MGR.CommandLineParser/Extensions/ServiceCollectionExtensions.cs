using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Extensions methods for the type <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default services for the command line parsing.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCommandLineParser(this IServiceCollection services)
        {
            services.AddLogging();

            services.TryAddSingleton<IConsole, DefaultConsole>();
            services.TryAddSingleton<IAssemblyProvider, CurrentDirectoryAssemblyProvider>();
            services.TryAddScoped<ICommandTypeProvider, AssemblyBrowsingClassBasedCommandTypeProvider>();
            services.TryAddScoped<IClassBasedCommandActivator, ClassBasedDependencyResolverCommandActivator>();
            services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();
            services.TryAddSingleton<IOptionAlternateNameGenerator, KebabCaseOptionAlternateNameGenerator>();

            services
                    .AddSingleton<IConverter, BooleanConverter>()
                    .AddSingleton<IConverter, ByteConverter>()
                    .AddSingleton<IConverter, CharConverter>()
                    .AddSingleton<IConverter, DateTimeConverter>()
                    .AddSingleton<IConverter, DecimalConverter>()
                    .AddSingleton<IConverter, DoubleConverter>()
                    .AddSingleton<IConverter, EnumConverter>()
                    .AddSingleton<IConverter, FileSystemInfoConverter>()
                    .AddSingleton<IConverter, GuidConverter>()
                    .AddSingleton<IConverter, Int16Converter>()
                    .AddSingleton<IConverter, Int32Converter>()
                    .AddSingleton<IConverter, Int64Converter>()
                    .AddSingleton<IConverter, SingleConverter>()
                    .AddSingleton<IConverter, StringConverter>()
                    .AddSingleton<IConverter, TimeSpanConverter>()
                    .AddSingleton<IConverter, UriConverter>();

            return services;
        }
    }
}
