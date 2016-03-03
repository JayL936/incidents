﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebApplication1.Models
{
    public class IncidentType
    {
        [Key]
        public int TypeID { get; set; }

        public string Name { get; set; }
        public string IconUrl { get; set; }
        
    }
}