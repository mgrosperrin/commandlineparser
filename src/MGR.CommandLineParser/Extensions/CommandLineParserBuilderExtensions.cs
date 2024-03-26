using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods for the type <see cref="CommandLineParserBuilder" />.
/// </summary>
public static class CommandLineParserBuilderExtensions
{
    /// <summary>
    /// Add a <see cref="ICommandTypeProvider"/> based on browsing all types in all loaded assemblies after having loaded all assemblies present on the current folder.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineParserBuilder"/> to configure.</param>
    /// <remarks>If you are running on .NET Core, do not use this method.</remarks>
    /// <returns>The <see cref="CommandLineParserBuilder" /> so that additional calls can be chained.</returns>
    public static CommandLineParserBuilder AddClassBasedCommands(this CommandLineParserBuilder builder)
    {
        builder.Services.AddSingleton<IAssemblyProvider, CurrentDirectoryAssemblyProvider>();
        builder.Services.TryAddScoped<ICommandTypeProvider, AssemblyBrowsingClassBasedCommandTypeProvider>();
        builder.Services.TryAddScoped<IClassBasedCommandActivator, ClassBasedDependencyResolverCommandActivator>();

        return builder;
    }

    /// <summary>
    /// Add a <see cref="ICommandTypeProvider"/> based on browsing all types in the assembly containing the specified <typeparamref name="TCommandHandler"/>.
    /// </summary>
    /// <typeparam name="TCommandHandler">The command type used to specify the assembly to browse.</typeparam>
    /// <typeparam name="TCommandData">The command type used to specify the assembly to browse.</typeparam>
    /// <param name="builder">The <see cref="CommandLineParserBuilder"/> to configure.</param>
    /// <returns>The <see cref="CommandLineParserBuilder" /> so that additional calls can be chained.</returns>
    public static CommandLineParserBuilder AddCommands<TCommandHandler, TCommandData>(this CommandLineParserBuilder builder)
        where TCommandHandler : class, ICommandHandler<TCommandData>
        where TCommandData : CommandData, new()
    {
        builder.Services.AddSingleton<IAssemblyProvider>(new DefaultAssemblyProvider(typeof(TCommandHandler).Assembly));
        builder.Services.TryAddScoped<ICommandTypeProvider, AssemblyBrowsingClassBasedCommandTypeProvider>();
        builder.Services.TryAddScoped<IClassBasedCommandActivator, ClassBasedDependencyResolverCommandActivator>();

        return builder;
    }
}
