using System;
using System.Collections.Generic;
using System.Linq;

namespace MGR.CommandLineParser.Extensibility.DependencyInjection
{
    internal class DefaultDependencyResolverScope : IDependencyResolverScope
    {
        private readonly Dictionary<Type, Func<IDependencyResolverScope, IEnumerable<object>>>
            _multiplyRegistredDependencies;

        private readonly Dictionary<Type, Func<IDependencyResolverScope, object>> _singlyRegistredDependencies;

        public DefaultDependencyResolverScope(
            Dictionary<Type, Func<IDependencyResolverScope, IEnumerable<object>>> multiplyRegistredDependencies,
            Dictionary<Type, Func<IDependencyResolverScope, object>> singlyRegistredDependencies)
        {
            _multiplyRegistredDependencies = multiplyRegistredDependencies;
            _singlyRegistredDependencies = singlyRegistredDependencies;
        }

        /// <inheritdoc />
        public T ResolveDependency<T>() where T : class
        {
            if (!_singlyRegistredDependencies.ContainsKey(typeof (T)))
            {
                return null;
            }
            var serviceFactory = _singlyRegistredDependencies[typeof (T)];
            if (serviceFactory == null)
            {
                return null;
            }
            var serviceObject = serviceFactory(this);
            var service = serviceObject as T;
            return service;
        }

        /// <inheritdoc />
        public IEnumerable<T> ResolveDependencies<T>() where T : class
        {
            if (!_multiplyRegistredDependencies.ContainsKey(typeof (T)))
            {
                return Enumerable.Empty<T>();
            }
            var servicesFactory = _multiplyRegistredDependencies[typeof (T)];
            if (servicesFactory == null)
            {
                return Enumerable.Empty<T>();
            }
            var servicesObject = servicesFactory(this);
            var services = servicesObject.OfType<T>();
            return services;
        }
    }
}