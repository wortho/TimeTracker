using System;
using System.Collections.Generic;

namespace TimeTracker.Web
{
    public class SimpleLoggerOptions
    {
        public IList<string> RequestKeys { get; set; }
        public IList<string> ResponseKeys { get; set; }
        public Action<string, object> Log { get; set; }
    }
}