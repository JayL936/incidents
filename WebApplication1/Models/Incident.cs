using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Incident
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Add date")]
        [DataType(DataType.Date)]
        //   [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime AddDate { get; set; }

        [Required]
        [Display(Name = "Incident date")]
        [DataType(DataType.Date)]
        //  [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime DateOfIncident { get; set; }

        [Required]
        [Display(Name = "Incident time")]
        public TimeSpan TimeOfIncident { get; set; }

      //  [Required]
        public string Type { get; set; }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public double Lat { get; set; }

        [Required]
        [Display(Name = "Longtitude")]
        public double Long { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public int TypeID { get; set; }
        public virtual IncidentType IncidentType { get; set; }

   //     [Required]
        public virtual ICollection<ServiceParticipation> ServiceParticipations { get; set; }
    }
}