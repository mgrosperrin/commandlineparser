using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Static class that provides and defines the current <see cref="IDependencyResolver"/>.
    /// </summary>
    [PublicAPI]
    public static class DependencyResolver
    {
        /// <summary>
        /// Gets the current <see cref="IDependencyResolver"/>.
        /// </summary>
        [NotNull]
        public static IDependencyResolver Current { get; private set; } = DefaultDependencyResolver.Current;

        /// <summary>
        /// Defines the current <see cref="IDependencyResolver"/>.
        /// </summary>
        /// <param name="dependencyResolver">The new current <see cref="IDependencyResolver"/> (cannot be null).</param>
        public static void SetDependencyResolver([NotNull]IDependencyResolver dependencyResolver)
        {
            Guard.NotNull(dependencyResolver, nameof(dependencyResolver));

            Current = dependencyResolver;
        }
    }
}