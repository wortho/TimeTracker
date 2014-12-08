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
    public class ProjectsController : ApiController
    {
        private readonly ITimeTrackerContext context = new TimeTrackerContext("DefaultConnection");

        public ProjectsController()
        {
        }

        public ProjectsController(ITimeTrackerContext context)
        {
            this.context = context;
        }

        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {
            return context.Projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            context.SetModified(project);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return context.Projects.Count(e => e.Id == id) > 0;
        }


        [Route("api/Projects/{id}/TimeEntries")]
        public IQueryable<TimeEntry> GetTimeEntriesForProject(int id)
        {
            return context.TimeEntries.AsQueryable()
                .Where(t => t.ProjectId == id)
                .OrderBy(t => t.StartTime);
        }
    }
}