using System;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser
{
    internal class ServiceResolverCommandActivator : ICommandActivator
    {
        private readonly MethodInfo _genericResolveServiceMethodInfo;

        public ServiceResolverCommandActivator()
        {
            var serviceResolverType = typeof (IDependencyResolverScope);
            _genericResolveServiceMethodInfo = serviceResolverType.GetMethod(nameof(IDependencyResolverScope.ResolveDependency));
        }


        public ICommand ActivateCommand(Type commandType)
        {
            var resolveServiceMethod = _genericResolveServiceMethodInfo.MakeGenericMethod(commandType);
            var command = resolveServiceMethod.Invoke(DependencyResolver.Current, null);
            return command as ICommand;
        }
    }
}