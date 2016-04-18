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
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool confirmed { get; set; }
    }
}