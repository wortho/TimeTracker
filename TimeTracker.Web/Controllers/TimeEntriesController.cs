using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using TimeTracker.Model;

namespace TimeTracker.Web.Controllers
{
    [Authorize]
    public class TimeEntriesController : TimeTrackerController
    {
        internal TimeEntriesController()
        {
            GetCurrentUserId = () => User.Identity.GetUserId();
        }

        public TimeEntriesController(ITimeTrackerContext context)
        {
            this.Context = context;
        }
        
        public Func<string> GetCurrentUserId;

        // GET: api/TimeEntries
        public IQueryable<TimeEntry> GetTimeEntries()
        {
            SetNoCacheHeader();
            return Context.TimeEntries;
        }

        // GET: api/TimeEntries/5
        [ResponseType(typeof(TimeEntry))]
        public async Task<IHttpActionResult> GetTimeEntry(int id)
        {
            SetNoCacheHeader();
            TimeEntry timeEntry = await Context.TimeEntries.FindAsync(id);
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

            Context.SetModified(timeEntry);

            try
            {
                await Context.SaveChangesAsync();
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

            timeEntry.UserId = GetCurrentUserId();

            Context.TimeEntries.Add(timeEntry);
            await Context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = timeEntry.Id }, timeEntry);
        }

        // DELETE: api/TimeEntries/5
        [ResponseType(typeof(TimeEntry))]
        public async Task<IHttpActionResult> DeleteTimeEntry(int id)
        {
            TimeEntry timeEntry = await Context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            Context.TimeEntries.Remove(timeEntry);
            await Context.SaveChangesAsync();

            return Ok(timeEntry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimeEntryExists(int id)
        {
            return Context.TimeEntries.Count(e => e.Id == id) > 0;
        }
    }
}