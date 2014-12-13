using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TimeTracker.Model;

namespace TimeTracker.Web.Controllers
{
    [Authorize]
    public class ProjectsController : TimeTrackerController
    {
        internal ProjectsController()
        {
            // default constructor for activator
        }

        public ProjectsController(ITimeTrackerContext context)
        {
            this.Context = context;
        }

        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {
            return Context.Projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await Context.Projects.FindAsync(id);
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

            Context.SetModified(project);

            try
            {
                await Context.SaveChangesAsync();
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

            Context.Projects.Add(project);
            await Context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await Context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            Context.Projects.Remove(project);
            await Context.SaveChangesAsync();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return Context.Projects.Count(e => e.Id == id) > 0;
        }


        [Route("api/Projects/{id}/TimeEntries")]
        public async Task<IHttpActionResult> GetTimeEntriesForProject(int id)
        {
            Project project = await Context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            await Context.Entry(project).Collection("TimeEntries").LoadAsync();

            return Ok(project.TimeEntries);
        }
    }
}