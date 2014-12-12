using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTracker.Web.Controllers;
using TimeTracker.Model;

namespace TimeTracker.Web.Tests.Controllers
{
    [TestClass]
    public class ProjectsControllerTest
    {
        [TestMethod]
        public async Task Get()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);

            // Act
            IQueryable<Project> result = controller.GetProjects();

            // Assert
            Assert.IsNotNull(result);
            var actual = await result.ToListAsync();
            Assert.AreEqual(4, actual.Count);

            var cust = actual.First();
            Assert.IsNotNull(cust);
        }

        private static Mock<ITimeTrackerContext> CreateTestProjectSet()
        {
            var context = new Mock<ITimeTrackerContext>();
            var projects = TestModelData.CreateTestProjects();
            var projectsDbSet = new Mock<DbSet<Project>>().SetupData(projects, objects => projects[0]);
            context.Setup(p => p.Projects).Returns(projectsDbSet.Object);
            return context;
        }
        
        [TestMethod]
        public async Task GetById()
        {
            // Arrange
            var projects = TestModelData.CreateTestProjects();
            var projectDbSet = new Mock<DbSet<Project>>().SetupData(projects, objects => projects[0]);
            projectDbSet.Setup(s => s.FindAsync(21)).ReturnsAsync(projects[2]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Projects).Returns(projectDbSet.Object);
            var controller = new ProjectsController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetProject(21);

            // Assert
            Assert.IsNotNull(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<Project>;
            Assert.IsNotNull(contentResult);

            var actual = contentResult.Content;
            Assert.IsNotNull(actual);
            Assert.AreEqual(21, actual.Id);
        }

        [TestMethod]
        public async Task GetByIdReturnsNotFound()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetProject(1010);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);

            var project22 = new Project
            {
                Id = 22,
                CustomerId = 2,
                Name = "Project22"
            };

            // Act
            IHttpActionResult actionResult = await controller.PostProject(project22);

            // Assert
            Assert.IsNotNull(actionResult);
            //Assert.IsTrue(actionResult is OkNegotiatedContentResult<Customer>);
        }


        [TestMethod]
        public async Task PutWrongId()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);


            var project11Update = new Project
            {
                Id = 11,
                CustomerId = 1,
                Name = "Project11Updated"
            };

            // Act
            IHttpActionResult actionResult = await controller.PutProject(21, project11Update);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);


            var project11Update = new Project
            {
                Id = 11,
                CustomerId = 1,
                Name = "Project11Updated"
            };

            // Act
            IHttpActionResult actionResult = await controller.PutProject(11, project11Update);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task PutNonExistentId()
        {
            // Arrange
            var context = CreateTestProjectSet();
            var controller = new ProjectsController(context.Object);


            var project11Update = new Project
            {
                Id = 41,
                CustomerId = 1,
                Name = "Project11Updated"
            };

            // Act
            IHttpActionResult actionResult = await controller.PutProject(41, project11Update);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var projects = TestModelData.CreateTestProjects();
            var projectDbSet = new Mock<DbSet<Project>>().SetupData(projects, objects => projects[0]);
            projectDbSet.Setup(s => s.FindAsync(21)).ReturnsAsync(projects[1]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Projects).Returns(projectDbSet.Object);
            var controller = new ProjectsController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.DeleteProject(21);

            // Assert
            var contentResult = actionResult as OkNegotiatedContentResult<Project>;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void GetTimeEntriesForProject()
        {

            // Arrange
            var projects = TestModelData.CreateTestProjects();
            var timeEntries = TestModelData.CreateTimeEntriesForProject(projects[0]);
            timeEntries.AddRange(TestModelData.CreateTimeEntriesForProject(projects[1]));
            timeEntries.AddRange(TestModelData.CreateTimeEntriesForProject(projects[2]));
            var projectDbSet = new Mock<DbSet<Project>>().SetupData(projects, objects => projects[0]);
            var timeEntryDbSet = new Mock<DbSet<TimeEntry>>().SetupData(timeEntries, objects => timeEntries[0]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Projects).Returns(projectDbSet.Object);
            context.Setup(c => c.TimeEntries).Returns(timeEntryDbSet.Object);
            var controller = new ProjectsController(context.Object);

            // Act
            var actual = controller.GetTimeEntriesForProject(projects[0].Id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Result);
        }
    }
}
