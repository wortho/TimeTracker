using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Owin;

namespace TimeTracker.Web
{
    public class SimpleLogger
    {
        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly SimpleLoggerOptions _options;

        public SimpleLogger(Func<IDictionary<string, object>, Task> next, SimpleLoggerOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            foreach (var key in _options.RequestKeys)
            {
                _options.Log(key, environment[key]);
            }

            await _next(environment);

            foreach (var key in _options.ResponseKeys)
            {
                _options.Log(key, environment[key]);
            }
        }

        public static void ConfigureLogger(IAppBuilder app)
        {
            var options = new SimpleLoggerOptions
            {
                Log = (key, value) => Debug.WriteLine("{0}:{1}", key, value),
                RequestKeys = new[] {"owin.RequestPath", "owin.RequestMethod"},
                ResponseKeys = new[] {"owin.ResponseStatusCode"}
            };

            app.UseSimpleLogger(options);
        }
    }
}