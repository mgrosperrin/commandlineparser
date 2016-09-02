using System;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Implementation of <see cref="ICommandActivator"/> based on <see cref="DependencyResolver"/>.
    /// </summary>
    public sealed class DependencyResolverCommandActivator : ICommandActivator
    {
        private readonly IDependencyResolverScope _dependencyResolverScope;
        private static readonly MethodInfo GenericResolveServiceMethodInfo = typeof(IDependencyResolverScope).GetMethod(nameof(IDependencyResolverScope.ResolveDependency));

        /// <summary>
        /// Creates a new instance of <see cref="DependencyResolverCommandActivator"/>.
        /// </summary>
        public DependencyResolverCommandActivator(IDependencyResolverScope dependencyResolverScope)
        {
            _dependencyResolverScope = dependencyResolverScope;
        }

        /// <inheritdoc />
        public ICommand ActivateCommand(Type commandType)
        {
            var resolveServiceMethod = GenericResolveServiceMethodInfo.MakeGenericMethod(commandType);
            var command = resolveServiceMethod.Invoke(_dependencyResolverScope, null);
            return command as ICommand;
        }
    }
}