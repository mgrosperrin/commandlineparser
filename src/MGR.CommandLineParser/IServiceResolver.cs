using System.Collections.Generic;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Defines the methods for the resolution of the services used by the CommandLineParser.
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// Resolves singly registred services.
        /// </summary>
        /// <typeparam name="T">The type of the service to resolve.</typeparam>
        /// <returns>The resolved service.</returns>
        T ResolveService<T>() where T : class;
        /// <summary>
        /// Resolves multiply registred services.
        /// </summary>
        /// <typeparam name="T">The type of the services to resolve.</typeparam>
        /// <returns>The resolved services.</returns>
        IEnumerable<T> ResolveServices<T>() where T : class;
    }
}