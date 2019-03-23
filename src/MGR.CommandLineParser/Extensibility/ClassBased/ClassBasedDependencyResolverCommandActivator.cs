using System;
using System.Linq;
using System.Reflection;
using MGR.CommandLineParser.Command;

namespace MGR.CommandLineParser.Extensibility.ClassBased
{
    /// <summary>
    ///     Implementation of <see cref="IClassBasedCommandActivator" /> based on <see cref="IServiceProvider" />.
    /// </summary>
    public sealed class ClassBasedDependencyResolverCommandActivator : IClassBasedCommandActivator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Creates a new instance of <see cref="ClassBasedDependencyResolverCommandActivator" />.
        /// </summary>
        public ClassBasedDependencyResolverCommandActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ICommand ActivateCommand(Type commandType)
        {
            var commandInstance = _serviceProvider.GetService(commandType);
            if (commandInstance == null)
            {
                var constructors = commandType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .OrderBy(constructor => constructor.GetParameters().Length);
                foreach (var constructorInfo in constructors)
                {
                    try
                    {
                        var parameterInfos = constructorInfo.GetParameters();
                        var parameters = new object[parameterInfos.Length];
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            var parameterInfo = parameterInfos[i];
                            var parameterValue = _serviceProvider.GetService(parameterInfo.ParameterType);
                            if (parameterValue != null || parameterInfo.TryGetDefaultValue(out parameterValue))
                            {
                                parameters[i] = parameterValue;
                            }
                        }

                        commandInstance = constructorInfo.Invoke(parameters);
                        if (commandInstance != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        // ignored: this is normal: we try to instantiate a command with each constructor.
                    }
                }
            }
            var command = commandInstance as ICommand;
            return command;
        }
    }
}