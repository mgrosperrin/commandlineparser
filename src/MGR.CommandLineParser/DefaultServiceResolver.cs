using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
    /// <summary>
    ///     Default implementation of the <see cref="IServiceResolver" />.
    /// </summary>
    /// <remarks>This implementation uses a simple map to find the service to resolve.</remarks>
    public class DefaultServiceResolver : IServiceResolver
    {
        private static readonly List<IConverter> Converters = new List<IConverter>
        {
            new BooleanConverter(),
            new ByteConverter(),
            new CharConverter(),
            new DateTimeConverter(),
            new DecimalConverter(),
            new DoubleConverter(),
            new EnumConverter(),
            new GuidConverter(),
            new Int16Converter(),
            new Int32Converter(),
            new Int64Converter(),
            new SingleConverter(),
            new StringConverter(),
            new TimeSpanConverter(),
            new UriConverter()
        };

        private static readonly Dictionary<Type, Func<IEnumerable<object>>> MultiplyRegistredServices =
            new Dictionary<Type, Func<IEnumerable<object>>>
            {
                {typeof (IConverter), () => Converters}
            };

        private static readonly Dictionary<Type, Func<object>> SinglyRegistredServices = new Dictionary<Type, Func<object>>
        {
            {typeof (IConsole), () => new DefaultConsole()},
            {
                typeof (ICommandProvider), () => new DirectotyBrowsingCommandProvider(
                    ServiceResolver.Current.ResolveService<IAssemblyFileProvider>(),
                    ServiceResolver.Current.ResolveService<ICommandActivator>())
            },
            {typeof (ICommandActivator), () => new BasicCommandActivator()},
            {typeof (IAssemblyFileProvider), () => new CurrentDirectoryAssemblyFileProvider()}
        };

        /// <inheritdoc />
        public T ResolveService<T>() where T : class
        {
            if (!SinglyRegistredServices.ContainsKey(typeof (T)))
            {
                return null;
            }
            var serviceFactory = SinglyRegistredServices[typeof (T)];
            if (serviceFactory == null)
            {
                return null;
            }
            var serviceObject = serviceFactory();
            var service = serviceObject as T;
            return service;
        }

        /// <inheritdoc />
        public IEnumerable<T> ResolveServices<T>() where T : class
        {
            if (!MultiplyRegistredServices.ContainsKey(typeof (T)))
            {
                return null;
            }
            var servicesFactory = MultiplyRegistredServices[typeof (T)];
            if (servicesFactory == null)
            {
                return null;
            }
            var servicesObject = servicesFactory();
            var services = servicesObject.OfType<T>();
            return services;
        }

        /// <summary>
        ///     Register a service.
        /// </summary>
        /// <typeparam name="T">The type of the contract of the service.</typeparam>
        /// <param name="serviceFactory">A factory to get the implementation of the service.</param>
        [PublicAPI]
        public static void RegisterService<T>([NotNull] Func<T> serviceFactory) where T : class
        {
            Guard.NotNull(serviceFactory, nameof(serviceFactory));

            var serviceType = typeof (T);
            if (SinglyRegistredServices.ContainsKey(serviceType))
            {
                SinglyRegistredServices[serviceType] = () => serviceFactory?.Invoke();
            }
            else
            {
                SinglyRegistredServices.Add(serviceType, () => serviceFactory?.Invoke());
            }
        }

        /// <summary>
        ///     Register a collection of services.
        /// </summary>
        /// <typeparam name="T">The type of the contract of the service.</typeparam>
        /// <param name="servicesFactory">A factory to get the implementations of the service.</param>
        public static void RegisterServices<T>([NotNull] Func<IEnumerable<T>> servicesFactory) where T : class
        {
            var serviceType = typeof (T);
            if (MultiplyRegistredServices.ContainsKey(serviceType))
            {
                MultiplyRegistredServices[serviceType] = () => servicesFactory?.Invoke();
            }
            else
            {
                MultiplyRegistredServices.Add(serviceType, () => servicesFactory?.Invoke());
            }
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

            var oldConverter = Converters.FirstOrDefault(conv => conv.CanConvertTo(converter.TargetType));
            if (oldConverter == null)
            {
                Converters.Add(converter);
            }
            else if (overwriteExisting)
            {
                Converters.Remove(oldConverter);
                Converters.Add(converter);
            }
        }

        /// <summary>
        ///     Remove a <see cref="IConverter" />.
        /// </summary>
        /// <param name="converter">The <see cref="IConverter"/> to remove.</param>
        [PublicAPI]
        public static void RemoveConverter([NotNull] IConverter converter)
        {
            Guard.NotNull(converter, nameof(converter));

            Converters.Remove(converter);
        }
    }
}