using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions methods for the type <see cref="CommandLineParserBuilder" />.
    /// </summary>
    public static class CommandLineParserBuilderExtensions
    {
        /// <summary>
        /// Add a <see cref="ICommandTypeProvider"/> based on browsing all types in all loaded assemblies after having loaded all assemblies present on the current folder.
        /// </summary>
        /// <param name="builder">The <see cref="CommandLineParserBuilder"/> to configure.</param>
        /// <returns>The <see cref="CommandLineParserBuilder" /> so that additional calls can be chained.</returns>
        public static CommandLineParserBuilder AddClassBasedCommands(this CommandLineParserBuilder builder)
        {
            builder.Services.TryAddSingleton<IAssemblyProvider, CurrentDirectoryAssemblyProvider>();
            builder.Services.TryAddScoped<ICommandTypeProvider, AssemblyBrowsingClassBasedCommandTypeProvider>();
            builder.Services.TryAddScoped<IClassBasedCommandActivator, ClassBasedDependencyResolverCommandActivator>();

            return builder;
        }

        /// <summary>
        /// Add a <see cref="ICommandTypeProvider"/> based on browsing all types in the assembly containing the specified <typeparamref name="TCommand"/>.
        /// </summary>
        /// <typeparam name="TCommand">The command type used to specify the assembly to browse.</typeparam>
        /// <param name="builder">The <see cref="CommandLineParserBuilder"/> to configure.</param>
        /// <returns>The <see cref="CommandLineParserBuilder" /> so that additional calls can be chained.</returns>
        public static CommandLineParserBuilder AddCommands<TCommand>(this CommandLineParserBuilder builder)
            where TCommand : class, ICommand
        {
            builder.Services.TryAddSingleton<IAssemblyProvider>(new DefaultAssemblyProvider(typeof(TCommand).Assembly));
            builder.Services.TryAddScoped<ICommandTypeProvider, AssemblyBrowsingClassBasedCommandTypeProvider>();
            builder.Services.TryAddScoped<IClassBasedCommandActivator, ClassBasedDependencyResolverCommandActivator>();

            return builder;
        }
    }
}
