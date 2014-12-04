using System.Collections.Generic;

namespace TimeTracker.Web.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        
        public virtual Customer Customer { get; set; }

        public string Name { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; }
    }
}