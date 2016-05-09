using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class IncidentsViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }

        [Display(Name = "Add date")]
        [DataType(DataType.Date)]
        public DateTime AddDate { get; set; }

        [Display(Name = "Incident date")]
        [DataType(DataType.Date)]
        public DateTime DateOfIncident { get; set; }

        [Display(Name = "Incident time")]
        public TimeSpan TimeOfIncident { get; set; }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "Latitude")]
        public double Lat { get; set; }

        [Display(Name = "Longtitude")]
        public double Long { get; set; }
        public string IconUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public bool confirmed { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime startDate { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime endDate { get; set; }

        public List<RoleViewModel> Roles { get; set; }
        public IQueryable<Participant> Participants { get; set; }
    }
}