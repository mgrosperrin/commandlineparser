using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MGR.CommandLineParser.Converters;

namespace MGR.CommandLineParser
{
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
                {typeof(IConverter),()=> Converters}
            };

        private static readonly Dictionary<Type, Func<object>> SinglyRegistredServices = new Dictionary<Type, Func<object>>
        {
            {typeof (IConsole), () => new DefaultConsole()},
            {typeof (ICommandProvider), () => new DefaultCommandProvider()}
        };

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

        public static void RegisterService<T>([NotNull]Func<T> serviceFactory)
        {
            var serviceType = typeof (T);
            if (SinglyRegistredServices.ContainsKey(serviceType))
            {
                SinglyRegistredServices[serviceType] = () => serviceFactory();
            }
            else
            {
                SinglyRegistredServices.Add(serviceType, () => serviceFactory());
            }
        }

        public static void RegisterServices<T>(Func<IEnumerable<T>> servicesFactory)
        {
            var serviceType = typeof (T);
            if (MultiplyRegistredServices.ContainsKey(serviceType))
            {
                MultiplyRegistredServices[serviceType] = () => servicesFactory().OfType<object>();
            }
            else
            {
                MultiplyRegistredServices.Add(serviceType, () => servicesFactory().OfType<object>());
            }
        }

        public static void RegisterConverter(IConverter converter, bool overwriteExisting = false)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            IConverter oldConverter = Converters.FirstOrDefault(conv => conv.CanConvertTo(converter.TargetType));
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
        public static void RemoveConverter(IConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            Converters.Remove(converter);
        }
    }
}