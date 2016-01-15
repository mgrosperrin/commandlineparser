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
		public static IDependencyResolver Current { get { return _current; } private set { _current = value; } } 
		static IDependencyResolver _current = DefaultDependencyResolver.Current;

        /// <summary>
        /// Defines the current <see cref="IDependencyResolver"/>.
        /// </summary>
        /// <param name="dependencyResolver">The new current <see cref="IDependencyResolver"/> (cannot be null).</param>
		public static void SetDependencyResolver([NotNull]IDependencyResolver dependencyResolver)
        {
			// "nameof" is from C# 6.0 specification
			// http://stackoverflow.com/questions/31695900/what-is-the-purpose-of-nameof
            Guard.NotNull(dependencyResolver, "dependencyResolver");

            Current = dependencyResolver;
        }
    }
}