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
            var serviceResolverType = typeof (IServiceResolver);
            _genericResolveServiceMethodInfo = serviceResolverType.GetMethod(nameof(IServiceResolver.ResolveService));
        }


        public ICommand ActivateCommand(Type commandType)
        {
            var resolveServiceMethod = _genericResolveServiceMethodInfo.MakeGenericMethod(commandType);
            var command = resolveServiceMethod.Invoke(ServiceResolver.Current, null);
            return command as ICommand;
        }
    }
}