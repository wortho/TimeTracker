using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracker.Web.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}