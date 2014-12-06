using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TimeTracker.Web.Models;
using TimeTracker.Web.Repository;

namespace TimeTracker.Web.Controllers
{
    [Authorize]
    public class TimeEntriesController : ApiController
    {
        private readonly ITimeTrackerContext context = new TimeTrackerContext();

        public TimeEntriesController()
        {
            
        }

        public TimeEntriesController(ITimeTrackerContext context)
        {
            this.context = context;
        }

        // GET: api/TimeEntries
        public IQueryable<TimeEntry> GetTimeEntries()
        {
            return context.TimeEntries;
        }

        // GET: api/TimeEntries/5
        [ResponseType(typeof(TimeEntry))]
        public async Task<IHttpActionResult> GetTimeEntry(int id)
        {
            TimeEntry timeEntry = await context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            return Ok(timeEntry);
        }

        // PUT: api/TimeEntries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTimeEntry(int id, TimeEntry timeEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timeEntry.Id)
            {
                return BadRequest();
            }

            context.SetModified(timeEntry);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TimeEntries
        [ResponseType(typeof(TimeEntry))]
        public async Task<IHttpActionResult> PostTimeEntry(TimeEntry timeEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.TimeEntries.Add(timeEntry);
            await context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = timeEntry.Id }, timeEntry);
        }

        // DELETE: api/TimeEntries/5
        [ResponseType(typeof(TimeEntry))]
        public async Task<IHttpActionResult> DeleteTimeEntry(int id)
        {
            TimeEntry timeEntry = await context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            context.TimeEntries.Remove(timeEntry);
            await context.SaveChangesAsync();

            return Ok(timeEntry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimeEntryExists(int id)
        {
            return context.TimeEntries.Count(e => e.Id == id) > 0;
        }
    }
}