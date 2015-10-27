using System;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    /// <summary>
    /// Implementation of <see cref="ICommandActivator"/> based on <see cref="DependencyResolver"/>.
    /// </summary>
    public class DependencyResolverCommandActivator : ICommandActivator
    {
        private readonly MethodInfo _genericResolveServiceMethodInfo;

        /// <summary>
        /// Creates a new instance of <see cref="DependencyResolverCommandActivator"/>.
        /// </summary>
        public DependencyResolverCommandActivator()
        {
            var serviceResolverType = typeof (IDependencyResolverScope);
            _genericResolveServiceMethodInfo = serviceResolverType.GetMethod(nameof(IDependencyResolverScope.ResolveDependency));
        }

        /// <inheritdoc />
        public ICommand ActivateCommand(Type commandType)
        {
            var resolveServiceMethod = _genericResolveServiceMethodInfo.MakeGenericMethod(commandType);
            var command = resolveServiceMethod.Invoke(DependencyResolver.Current, null);
            return command as ICommand;
        }
    }
}