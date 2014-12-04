using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TimeTracker.Web.Models;

namespace TimeTracker.Web.Repository
{
    public class TimeTrackerDatabaseInitializer : CreateDatabaseIfNotExists<TimeTrackerContext> // re-creates every time the server starts
    {
        protected override void Seed(TimeTrackerContext context)
        {
            base.Seed(context);

            //Generate customers and orders
            for (
                int i = 0; i < customerNames.Length; i++)
            {
                var nameGenderHost = SplitValue(customerNames[i]);
                var cityState = SplitValue(citiesStates[i]);
                var cust = new Customer
                {
                    Id = i + 1,
                    CompanyName =  String.Format("{0}{1}", nameGenderHost[2], nameGenderHost[0]),
                    ContactFirstName = nameGenderHost[0],
                    ContactLastName = nameGenderHost[1],
                    ContactEmail = String.Format("{0}.{1}@{2}", nameGenderHost[0], nameGenderHost[1], nameGenderHost[3]),
                    Address = addresses[i],
                    City = cityState[0],
                    Postcode = postCodeSeed + i
                };

                context.Customers.Add(cust);
                
                // Generate customer projects
                var custProjects = new List<Project>();
                for (int j = 0; j < 4; j++)
                {
                    var project = new Project
                    {
                        Id = cust.Id*100 + j*10,
                        CustomerId = cust.Id,
                        Name = string.Format("{0} Project {1}", cust.CompanyName, j)
                    };

                    context.Projects.Add(project);

                    // Generate project TimeEntries
                    var projectTimeEntries = GenerateTimeEntries(project);
                    context.TimeEntries.AddRange(projectTimeEntries);
                }

                
            }
        }

        private static IEnumerable<TimeEntry> GenerateTimeEntries(Project project)
        {
            return new List<TimeEntry>()
            {
                new TimeEntry()
                {
                    Id = project.Id + 1,
                    ProjectId = project.Id,
                    Description = "Entry1",
                    StartTime = DateTime.Now.AddHours(-6),
                    EndTime = DateTime.Now.AddHours(-2)
                }
            };
        }


        private static string[] SplitValue(string val)
        {
            return val.Split(',');
        }


        private const int postCodeSeed = 1000;

        static readonly string[] customerNames = 
        { 
            "Marcus,HighTower,Male,acmecorp.com", 
            "Jesse,Smith,Female,gmail.com", 
            "Albert,Einstein,Male,outlook.com", 
            "Dan,Wahlin,Male,yahoo.com", 
            "Ward,Bell,Male,gmail.com", 
            "Brad,Green,Male,gmail.com", 
            "Igor,Minar,Male,gmail.com", 
            "Miško,Hevery,Male,gmail.com", 
            "Michelle,Avery,Female,acmecorp.com", 
            "Heedy,Wahlin,Female,hotmail.com",
            "Thomas,Martin,Male,outlook.com",
            "Jean,Martin,Female,outlook.com",
            "Robin,Cleark,Female,acmecorp.com",
            "Juan,Paulo,Male,yahoo.com",
            "Gene,Thomas,Male,gmail.com",
            "Pinal,Dave,Male,gmail.com",
            "Fred,Roberts,Male,outlook.com",
            "Tina,Roberts,Female,outlook.com",
            "Cindy,Jamison,Female,gmail.com",
            "Robyn,Flores,Female,yahoo.com",
            "Jeff,Wahlin,Male,gmail.com",
            "Danny,Wahlin,Male,gmail.com",
            "Elaine,Jones,Female,yahoo.com"
        };
        static readonly string[] addresses = 
        { 
            "1234 Anywhere St.", 
            "435 Main St.", 
            "1 Atomic St.", 
            "85 Cedar Dr.", 
            "12 Ocean View St.", 
            "1600 Amphitheatre Parkway", 
            "1604 Amphitheatre Parkway", 
            "1607 Amphitheatre Parkway", 
            "346 Cedar Ave.", 
            "4576 Main St.", 
            "964 Point St.", 
            "98756 Center St.", 
            "35632 Richmond Circle Apt B",
            "2352 Angular Way", 
            "23566 Directive Pl.", 
            "235235 Yaz Blvd.", 
            "7656 Crescent St.", 
            "76543 Moon Ave.", 
            "84533 Hardrock St.", 
            "5687534 Jefferson Way",
            "346346 Blue Pl.", 
            "23423 Adams St.", 
            "633 Main St.", 
        };

        static string[] citiesStates = 
        { 
            "Phoenix,AZ", 
            "Encinitas,CA", 
            "Seattle,WA", 
            "Chandler,AZ", 
            "Dallas,TX", 
            "Orlando,FL", 
            "Carey,NC", 
            "Anaheim,CA", 
            "Dallas,TX", 
            "New York,NY",
            "White Plains,NY",
            "Las Vegas,NV",
            "Los Angeles,CA",
            "Portland,OR",
            "Seattle,WA",
            "Houston,TX",
            "Chicago,IL",
            "Atlanta,GA",
            "Chandler,AZ",
            "Buffalo,NY",
            "Albuquerque,AZ",
            "Boise,ID",
            "Salt Lake City,UT",
        };
    }
}