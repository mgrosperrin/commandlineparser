using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser.Extensibility.Converters;

namespace MGR.CommandLineParser.Extensibility.DependencyInjection
{
    /// <summary>
    ///     Default implementation of the <see cref="IDependencyResolver" />.
    /// </summary>
    /// <remarks>This implementation uses a simple map to find the service to resolve.</remarks>
    public class DefaultDependencyResolver : IDependencyResolver
    {
        internal static readonly DefaultDependencyResolver Current = new DefaultDependencyResolver();

        private readonly List<IConverter> _converters = Converters.Converters.GetAll();
        private ICommandTypeProvider _commandTypeProvider;
        private IHelpWriter _helpWriter;

        private readonly Dictionary<Type, Func<Func<IDependencyResolverScope, IEnumerable<object>>>> _multiplyRegistredDependencies =
            new Dictionary<Type, Func<Func<IDependencyResolverScope, IEnumerable<object>>>>();

        private readonly Dictionary<Type, Func<Func<IDependencyResolverScope, object>>> _singlyRegistredDependencies = new Dictionary<Type, Func<Func<IDependencyResolverScope, object>>>();

        private DefaultDependencyResolver()
        {
            SaveDependencies<IConverter>(() => _ => _converters);
            SaveDependency<IConsole>(() => _ => DefaultConsole.Instance);
            SaveDependency<ICommandActivator>(() => _ => BasicCommandActivator.Instance);
            SaveDependency<IAssemblyProvider>(() => _ => CurrentDirectoryAssemblyProvider.Instance);
            SaveDependency<ICommandTypeProvider>(() => _ => _commandTypeProvider ?? (_commandTypeProvider = new AssemblyBrowsingCommandTypeProvider(_.ResolveDependency<IAssemblyProvider>(), _.ResolveDependencies<IConverter>())));
            SaveDependency<IHelpWriter>(() => _ => _helpWriter ?? (_helpWriter = new DefaultHelpWriter(_.ResolveDependency<IConsole>(), _.ResolveDependency<ICommandTypeProvider>())));
        }

        /// <inheritdoc />
        public IDependencyResolverScope CreateScope() => new DefaultDependencyResolverScope(
                _multiplyRegistredDependencies.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()),
                _singlyRegistredDependencies.ToDictionary(kvp => kvp.Key, kvp => kvp.Value())
                );

        /// <summary>
        ///     Register a service.
        /// </summary>
        /// <typeparam name="T">The type of the contract of the service.</typeparam>
        /// <param name="serviceFactory">A factory to get the implementation of the service.</param>
        [PublicAPI]
        public static void RegisterDependency<T>([NotNull] Func<Func<IDependencyResolverScope, T>> serviceFactory) where T : class
        {
            Guard.NotNull(serviceFactory, nameof(serviceFactory));

            Current.SaveDependency(serviceFactory);
        }

        /// <summary>
        ///     Register a collection of services.
        /// </summary>
        /// <typeparam name="T">The type of the contract of the service.</typeparam>
        /// <param name="servicesFactory">A factory to get the implementations of the service.</param>
        [PublicAPI]
        public static void RegisterDependencies<T>([NotNull] Func<Func<IDependencyResolverScope, IEnumerable<T>>> servicesFactory) where T : class
        {
            Current.SaveDependencies(servicesFactory);
        }

        /// <summary>
        ///     Register a <see cref="IConverter" />.
        /// </summary>
        /// <param name="converter">The <see cref="IConverter" /> to register.</param>
        /// <remarks>Do not overwrite the converter.</remarks>
        [PublicAPI]
        public static void RegisterConverter([NotNull] IConverter converter)
        {
            RegisterConverter(converter, false);
        }

        private void SaveConverter([NotNull] IConverter converter, bool overwriteExisting)
        {
            var oldConverter = _converters.FirstOrDefault(conv => conv.CanConvertTo(converter.TargetType));
            if (oldConverter == null)
            {
                _converters.Add(converter);
            }
            else if (overwriteExisting)
            {
                _converters.Remove(oldConverter);
                _converters.Add(converter);
            }
        }

        /// <summary>
        ///     Register a <see cref="IConverter" />.
        /// </summary>
        /// <param name="converter">The <see cref="IConverter" /> to register.</param>
        /// <param name="overwriteExisting">
        ///     <code>true</code> to overwrite an existing <see cref="IConverter" /> for the same
        ///     target type. <code>false</code> otherwise.
        /// </param>
        [PublicAPI]
        public static void RegisterConverter([NotNull] IConverter converter, bool overwriteExisting)
        {
            Guard.NotNull(converter, nameof(converter));

            Current.SaveConverter(converter, overwriteExisting);
        }

        private void DeleteConverter([NotNull] IConverter converter)
        {
            _converters.Remove(converter);
        }

        /// <summary>
        ///     Remove a <see cref="IConverter" />.
        /// </summary>
        /// <param name="converter">The <see cref="IConverter" /> to remove.</param>
        [PublicAPI]
        public static void RemoveConverter([NotNull] IConverter converter)
        {
            Guard.NotNull(converter, nameof(converter));

            Current.DeleteConverter(converter);
        }

        private void SaveDependency<T>([NotNull] Func<Func<IDependencyResolverScope, T>> serviceFactory) where T : class
        {
            var serviceType = typeof(T);
            if (_singlyRegistredDependencies.ContainsKey(serviceType))
            {
                _singlyRegistredDependencies[serviceType] = serviceFactory;
            }
            else
            {
                _singlyRegistredDependencies.Add(serviceType, serviceFactory);
            }
        }
        private void SaveDependencies<T>([NotNull] Func<Func<IDependencyResolverScope, IEnumerable<T>>> servicesFactory) where T : class
        {
            var serviceType = typeof(T);
            if (_multiplyRegistredDependencies.ContainsKey(serviceType))
            {
                _multiplyRegistredDependencies[serviceType] = servicesFactory;
            }
            else
            {
                _multiplyRegistredDependencies.Add(serviceType, servicesFactory);
            }
            // This code is here to avoid the CC0052 issue with _commandTypeProvider (https://github.com/code-cracker/code-cracker/issues/544)
            if (_commandTypeProvider == null)
            {
                _commandTypeProvider = null;
            }
        }
    }
}