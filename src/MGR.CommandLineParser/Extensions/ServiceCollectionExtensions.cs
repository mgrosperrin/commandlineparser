using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MGR.CommandLineParser;

/// <summary>
/// Extensions methods for the type <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the default services for the command line parsing. No <see cref="ICommandTypeProvider"/> are registered. Add one or more <see cref="ICommandTypeProvider"/> via the returned <see cref="CommandLineParserBuilder"/>.
    /// </summary>
    /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the parser's services to.</param>
    /// <returns>The <see cref="CommandLineParserBuilder" /> so that <see cref="ICommandTypeProvider" /> can be registered.</returns>
    public static CommandLineParserBuilder AddCommandLineParser(this IServiceCollection services)
    {
        var builder = services.AddCommandLineParser(options => { });

        return builder;
    }

    /// <summary>
    /// Adds the default services for the command line parsing. No <see cref="ICommandTypeProvider"/> are registered. Add one or more <see cref="ICommandTypeProvider"/> via the returned <see cref="CommandLineParserBuilder"/>.
    /// </summary>
    /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the parser's services to.</param>
    /// <param name="configureParserOptions">An <see cref="Action{ParserOptions}"/> to configure the provided
    /// <see cref="ParserOptions"/>.</param>
    /// <returns>The <see cref="CommandLineParserBuilder" /> so that <see cref="ICommandTypeProvider" /> can be registered.</returns>
    public static CommandLineParserBuilder AddCommandLineParser(this IServiceCollection services, Action<ParserOptions> configureParserOptions)
    {
        services.AddLogging();
        services.AddOptions();

        services.Configure(configureParserOptions);
        services.TryAddSingleton<IConsole, DefaultConsole>();
        services.TryAddScoped<IHelpWriter, DefaultHelpWriter>();
        services.TryAddScoped<IParserFactory, DefaultParserFactory>();
        services.TryAddScoped(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IParserFactory>();
            return factory.CreateParser();
        });

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

        return new CommandLineParserBuilder(services);
    }
}
