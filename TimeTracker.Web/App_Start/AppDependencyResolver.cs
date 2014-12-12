using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace TimeTracker.Web
{
    public class AppDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, object> _mTypes = new Dictionary<Type, object>();

        public T Resolve<T>()
        {
            return (T)_mTypes[typeof(T)];
        }

        public void Register<T>(object obj)
        {
            if (obj is T == false)
            {
                throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", typeof(T).FullName));

            }
            _mTypes.Add(typeof(T), obj);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return Resolve<Type>();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return null;
        }

        public void Dispose()
        {
        }
    }
}