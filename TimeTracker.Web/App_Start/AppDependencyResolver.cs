using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using TimeTracker.Repository;

namespace TimeTracker.Web
{
    public class AppDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(TimeTrackerContext) ? new TimeTrackerContext("DefaultConnection") : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}