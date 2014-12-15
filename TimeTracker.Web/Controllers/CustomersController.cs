using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using TimeTracker.Model;

namespace TimeTracker.Web.Controllers
{
    [Authorize]
    public class CustomersController : TimeTrackerController
    {
        internal CustomersController()
        {

        }

        public CustomersController(ITimeTrackerContext context)
        {
            this.Context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IQueryable<Customer> GetCustomers()
        {
            SetNoCacheHeader();
            return Context.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            SetNoCacheHeader();
            Customer customer = await Context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [Route("api/Customers/{id}/Projects")]
        public IQueryable<Project> GetProjectsForCustomer(int id)
        {
            SetNoCacheHeader();
            return Context.Projects.AsQueryable()
                .Where(p => p.CustomerId == id)
                .OrderBy(p => p.Id);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            Context.SetModified(customer);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Context.Customers.Add(customer);
            await Context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {
            Customer customer = await Context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            Context.Customers.Remove(customer);
            await Context.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return Context.Customers.Count(e => e.Id == id) > 0;
        }
    }
}