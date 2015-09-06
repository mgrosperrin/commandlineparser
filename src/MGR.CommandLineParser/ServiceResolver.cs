using JetBrains.Annotations;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Static class that provides and defines the current <see cref="IServiceResolver"/>.
    /// </summary>
    [PublicAPI]
    public static class ServiceResolver
    {
        /// <summary>
        /// Gets the current <see cref="IServiceResolver"/>.
        /// </summary>
        [NotNull]
        public static IServiceResolver Current { get; private set; } = new DefaultServiceResolver();

        /// <summary>
        /// Defines the current <see cref="IServiceResolver"/>.
        /// </summary>
        /// <param name="serviceResolver">The new current <see cref="IServiceResolver"/> (cannot be null).</param>
        public static void SetServiceResolver([NotNull]IServiceResolver serviceResolver)
        {
            Guard.NotNull(serviceResolver, nameof(serviceResolver));

            Current = serviceResolver;
        }
    }
}