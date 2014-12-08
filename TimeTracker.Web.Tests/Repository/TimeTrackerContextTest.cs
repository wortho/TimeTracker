using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.Web.Models;
using TimeTracker.Web.Repository;

namespace TimeTracker.Web.Tests.Repository
{
    [TestClass]
    public class TimeTrackerContextTest
    {
        [TestMethod]
        public async Task TestModelRepository()
        {
            var context = new TimeTrackerContext("TestConnection");
            Assert.IsNotNull(context);

            var cust = new Customer
            {
                CompanyName = "Test Customer",
                ContactFirstName = "First Name",
                ContactLastName = "Last Name",
                ContactEmail = "cust@company.com",
                Address = "Address",
                City = "City",
                Postcode = 1000
            };

            context.Customers.Add(cust);
            Assert.IsNotNull(cust);
            Assert.AreEqual("Test Customer", cust.CompanyName);
            cust.CompanyName = cust.CompanyName + "Mod";
            context.SaveChanges();

            context.Entry(cust).Collection(customer => customer.Projects).Load();
            Assert.IsNotNull(cust.Projects);

            // Generate 4 projects for the customer
            for (int j = 0; j < 4; j++)
            {
                var project = new Project
                {
                    Customer = cust,
                    Name = string.Format("{0} Project {1}", cust.CompanyName, j)
                };

                context.Projects.Add(project);
            }
            await context.SaveChangesAsync();

            var proj = cust.Projects.First();

            var user = await CreateUserIfNotExists(context, "test@test.com");

            for (int i = 0; i < 4; i++)
            {
                TimeEntry entry = new TimeEntry() { ProjectId = proj.Id, UserId = user.Id, StartTime = DateTime.Now, EndTime = DateTime.UtcNow };
                context.TimeEntries.Add(entry);
            }
            await context.SaveChangesAsync();

            IQueryable<TimeEntry> entries = context.TimeEntries.AsQueryable()
                .Where(e => e.ProjectId == proj.Id)
                .OrderBy(e => e.StartTime);

            Assert.AreEqual(4, entries.Count());

            context.Entry(proj).Collection(p => p.TimeEntries).Load();
            Assert.AreEqual(4, proj.TimeEntries.Count);

            TimeEntry t = proj.TimeEntries.First();
            context.Entry(t).Reference(e => e.User).Load();

            Assert.AreEqual(user.UserName, t.User.UserName);

            context.Entry(t).Reference(e => e.Project).Load();
            Assert.AreEqual(t.Project.Id, proj.Id);
        }

        [TestMethod]
        public async Task CreateNewUser()
        {
            TimeTrackerContext context = new TimeTrackerContext("TestConnection");
            Assert.IsNotNull(context);

            var result = await CreateUserIfNotExists(context, "test123@test.com");
            Assert.IsNotNull(result);


        }

        private static async Task<ApplicationUser> CreateUserIfNotExists(TimeTrackerContext context, string userEmail)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = await manager.FindByEmailAsync(userEmail);
            if (user == null)
            {

                // create new test user
                user = new ApplicationUser() { UserName = userEmail, Email = userEmail };

                IdentityResult result = await manager.CreateAsync(user, "P@ssword1");
                Assert.IsTrue(result.Succeeded);
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            return user;
        }
    }
}
