using System;
using System.Collections.Generic;
using TimeTracker.Web.Models;

namespace TimeTracker.Web.Tests.Controllers
{
    internal static class TestModelData
    {
        internal static List<Customer> CreateTestCustomers()
        {

            return new List<Customer> 
            { 
                new Customer
                {
                    Id = 1,
                    CompanyName = "Company1",
                    Address = "Address1",
                    City = "City1",
                    ContactEmail = "ContactEmail1"
                },

                new Customer
                {
                    Id = 2,
                    CompanyName = "Company2",
                    Address = "Address2",
                    City = "City2",
                    ContactEmail = "ContactEmai2"
                },
                new Customer
                {
                    Id = 3,
                    CompanyName = "Company3",
                    Address = "Address3",
                    City = "City3",
                    ContactEmail = "ContactEmail3"
                }
            };
        }

        internal static List<Project> CreateTestProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Id = 11,
                    CustomerId = 1,
                    Name = "Project11"
                },
                new Project
                {
                    Id = 12,
                    CustomerId = 2,
                    Name = "Project11"
                },

                new Project
                {
                    Id = 21,
                    CustomerId = 2,
                    Name = "Project21"
                },
                new Project
                {
                    Id = 31,
                    CustomerId = 3,
                    Name = "Project21"
                }
            };

        }

        internal static List<TimeEntry> CreateTimeEntriesForProjects(List<Project> projects)
        {
            var timeEntries = CreateTimeEntriesForProject(projects[0]);
            timeEntries.AddRange(CreateTimeEntriesForProject(projects[1]));
            timeEntries.AddRange(CreateTimeEntriesForProject(projects[2]));
            return timeEntries;
        }

        internal static List<TimeEntry> CreateTimeEntriesForProject(Project project)
        {
            return new List<TimeEntry>()
            {
                new TimeEntry()
                {
                    Id = project.Id*100+1,
                    ProjectId = project.Id,
                    Description = "Entry1",
                    StartTime = DateTime.Now.AddHours(-6),
                    EndTime = DateTime.Now.AddHours(-2)
                },

                new TimeEntry()
                {
                    Id = project.Id*100+2,
                    ProjectId = project.Id,
                    Description = "Entry2",
                    StartTime = DateTime.Now.AddHours(-6),
                    EndTime = DateTime.Now.AddHours(-2)
                },
                
                new TimeEntry()
                {
                    Id = project.Id*100+3,
                    ProjectId = project.Id,
                    Description = "Entry3",
                    StartTime = DateTime.Now.AddHours(-6),
                    EndTime = DateTime.Now.AddHours(-2)
                }
            };
        }

    }
}
