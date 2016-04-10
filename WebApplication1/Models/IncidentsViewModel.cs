using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class IncidentsViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string IconUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}