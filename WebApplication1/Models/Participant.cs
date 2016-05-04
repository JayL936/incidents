using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Participant
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PESEL { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Type")]
        public int pTypeID { get; set; }
        public virtual ParticipantType ParticipantType { get; set; }

        [Display(Name = "Incident")]
        public int incidentID { get; set; }
        public virtual Incident Incident { get; set; }
    }
}