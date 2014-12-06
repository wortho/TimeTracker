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
    public class CustomersController : ApiController
    {
        private ITimeTrackerContext context = new TimeTrackerContext();

        public CustomersController()
        {
            
        }
        public CustomersController(ITimeTrackerContext context)
        {
            this.context = context;
        }

        // GET: api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            return context.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            Customer customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [Route("api/Customers/{id}/Projects")]
        public IQueryable<Project> GetProjectsForCustomer(int id)
        {
            return context.Projects.AsQueryable()
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

            context.SetModified(customer);

            try
            {
                await context.SaveChangesAsync();
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

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {
            Customer customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return context.Customers.Count(e => e.Id == id) > 0;
        }
    }
}