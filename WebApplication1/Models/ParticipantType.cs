using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ParticipantType
    {
        [Key]
        public int pTypeID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string pTypeName { get; set; }
    }
}