using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ServiceParticipation
    {
        //[Key]
        //public int ID { get; set; }

        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public string RoleId { get; set; }
     //   public virtual IdentityRole IdentityRole { get; set; }
    }
}