using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WebApplication1.Models
{
    public class IncidentType
    {
        [Key]
        public int TypeID { get; set; }

        [Required]
        public string Name { get; set; }

      //  [Required]
        [Display(Name = "Icon")]
        [DataType(DataType.ImageUrl)]
        public string IconUrl { get; set; }
    }
}