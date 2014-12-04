using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeTracker.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string ContactFirstName { get; set; }
        [StringLength(50)]
        public string ContactLastName { get; set; }
        [StringLength(100)]
        public string ContactEmail { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        public int Postcode { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}