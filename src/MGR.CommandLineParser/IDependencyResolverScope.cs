using System.Collections.Generic;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Represents a scope of the dependency injection container.
    /// The scope is used within a parsing operation.
    /// </summary>
    public interface IDependencyResolverScope
    {
        /// <summary>
        /// Resolves singly registred services.
        /// </summary>
        /// <typeparam name="T">The type of the service to resolve.</typeparam>
        /// <returns>The resolved service.</returns>
        T ResolveDependency<T>() where T : class;
        /// <summary>
        /// Resolves multiply registred services.
        /// </summary>
        /// <typeparam name="T">The type of the services to resolve.</typeparam>
        /// <returns>The resolved services.</returns>
        IEnumerable<T> ResolveDependencies<T>() where T : class;
    }
}