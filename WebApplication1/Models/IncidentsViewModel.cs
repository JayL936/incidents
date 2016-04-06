using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class IncidentsViewModel
    {

        public IEnumerable<Incident> Incident { get; set; }
        public IEnumerable<IncidentType> IncidentType { get; set; }

    }
}