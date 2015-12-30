using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class Incident
    {
        [Key]
        public int ID { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string Type { get; set; }
        public string About { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}