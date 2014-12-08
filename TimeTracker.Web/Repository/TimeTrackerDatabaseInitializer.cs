using System;
using System.Data.Entity;
using TimeTracker.Web.Models;

namespace TimeTracker.Web.Repository
{
    public class TimeTrackerDatabaseInitializer : CreateDatabaseIfNotExists<TimeTrackerContext>
    {
        protected override void Seed(TimeTrackerContext context)
        {
            base.Seed(context);

            GenerateSeedData(context);
        }

        public static void GenerateSeedData(ITimeTrackerContext context)
        {
            //Generate customers
            for (int i = 0; i < CustomerNames.Length; i++)
            {
                var names = SplitValue(CustomerNames[i]);
                var cust = new Customer
                {
                    CompanyName = String.Format("{0}{1}", names[1], names[0]),
                    ContactFirstName = names[0],
                    ContactLastName = names[1],
                    ContactEmail = String.Format("{0}.{1}@{2}", names[0], names[1], names[2]),
                    Address = Addresses[i],
                    City = CityNames[i],
                    Postcode = PostCodeSeed + i
                };

                context.Customers.Add(cust);

                GenerateProjects(context, cust);
            }
        }

        private static void GenerateProjects(ITimeTrackerContext context, Customer cust)
        {
            // Generate 4 projects for each customer
            for (int j = 0; j < 4; j++)
            {
                var project = new Project
                {
                    Customer = cust,
                    Name = string.Format("{0} Project {1}", cust.CompanyName, j)
                };

                context.Projects.Add(project);
            }
        }


        private static string[] SplitValue(string val)
        {
            return val.Split(',');
        }


        private const int PostCodeSeed = 1000;

        private static readonly string[] CustomerNames = 
        { 
            "John,Smith,gmail.com", 
            "Jane,Doe,outlook.com", 
            "Bob,Smith,yahoo.com", 
            "Jens,Jensen,gmail.com", 
            "Lars,Larsen,gmail.com", 
            "Peter,Petersen,gmail.com"
        };
        private static readonly string[] Addresses = 
        { 
            "1 Main St.", 
            "100 High St.", 
            "99 First St.", 
            "Hovedgaden 10", 
            "Strandvej 5", 
            "Bakkevej 8"
        };

        private static readonly string[] CityNames = 
        { 
            "London", 
            "New York", 
            "Seattle", 
            "København", 
            "Esbjerg", 
            "Helsingør"
        };
    }
}