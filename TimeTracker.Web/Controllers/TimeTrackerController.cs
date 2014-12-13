using System.Web.Http;
using TimeTracker.Model;
using TimeTracker.Repository;

namespace TimeTracker.Web.Controllers
{
    public class TimeTrackerController : ApiController
    {
        protected ITimeTrackerContext Context { get; set; }

        public TimeTrackerController()
        {
            Context = TimeTrackerContext.Create();
        }
    }
}