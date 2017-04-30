namespace MGR.CommandLineParser.Extensibility.DependencyInjection
{
    /// <summary>
    ///     Represents a dependency injection container.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        ///     Creates a new resolution scope.
        /// </summary>
        /// <returns>A new scope</returns>
        IDependencyResolverScope CreateScope();
    }
}