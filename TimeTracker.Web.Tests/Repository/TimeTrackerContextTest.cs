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
            TimeTrackerContext context = new TimeTrackerContext("TestConnection");
            Assert.IsNotNull(context);

            Customer cust = context.Customers.First();
            Assert.IsNotNull(cust);
            Assert.AreEqual(1, cust.Id);
            cust.CompanyName = cust.CompanyName + "Mod";
            context.SaveChanges();

            context.Entry(cust).Collection(customer => customer.Projects).Load();
            Assert.IsNotNull(cust.Projects);

            Project proj = cust.Projects.First();

            //var user = new ApplicationUser() { UserName = "test@test.com", Email = "test@test.com"};
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = await manager.FindByEmailAsync("test@test.com");
            Assert.IsNotNull(user);

            for (int i = 0; i < 4; i++)
            {
                TimeEntry entry = new TimeEntry() { ProjectId = proj.Id, User = user, StartTime = DateTime.Now, EndTime = DateTime.UtcNow };
                context.TimeEntries.Add(entry); 
            }
            context.SaveChanges();

            context.Entry(proj).Collection(p => p.TimeEntries).Load();
            Assert.AreEqual(4, proj.TimeEntries.Count);

            TimeEntry t = proj.TimeEntries.First();
            context.Entry(t).Reference(e => e.User).Load();

            Assert.AreEqual(user.UserName,t.User.UserName);

            context.Entry(t).Reference(e => e.Project).Load();
            Assert.AreEqual(t.Project.Id, proj.Id);

        }

        [TestMethod]
        public async Task CreateUser()
        {
            TimeTrackerContext context = new TimeTrackerContext("TestConnection");
            Assert.IsNotNull(context);

            var user = new ApplicationUser() { UserName = "test@test.com", Email = "test@test.com" };
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            IdentityResult result = await manager.CreateAsync(user, "P@ssword1");
            Assert.IsTrue(result.Succeeded);

        }
    }
}
