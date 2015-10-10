namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Represents a dependency injection container.
    /// </summary>
    public interface IDependencyResolver : IDependencyResolverScope
    {
        /// <summary>
        ///     Creates a new resolution scope.
        /// </summary>
        /// <returns>A new scope</returns>
        IDependencyResolverScope CreateScope();
    }
}