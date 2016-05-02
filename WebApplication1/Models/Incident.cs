﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Incident
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Add date")]
        [DataType(DataType.Date)]
     //   [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime AddDate { get; set; }

        [Display(Name = "Incident date")]
        [DataType(DataType.Date)]
      //  [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime DateOfIncident { get; set; }

        [Display(Name = "Incident time")]
        public TimeSpan TimeOfIncident { get; set; }

        public string Type { get; set; }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "Latitude")]
        public double Lat { get; set; }

        [Display(Name = "Longtitude")]
        public double Long { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public int TypeID { get; set; }
        public virtual IncidentType IncidentType { get; set; }

        public virtual ICollection<ServiceParticipation> ServiceParticipations { get; set; }
    }
}