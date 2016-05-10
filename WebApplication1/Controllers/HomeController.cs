using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        Context db = new Context();
        Context dbt = new Context();

        public ActionResult Index()
        {
            DateTime incidentsFromDate = DateTime.Now.AddDays(-30);
            DateTime incidentsToDate = DateTime.Now;
            ViewBag.endDate = incidentsToDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            ViewBag.startDate = incidentsFromDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            List<IncidentsViewModel> list = new List<IncidentsViewModel>();

            var incidents = (from i in db.Incidents
                             where (i.DateOfIncident >= incidentsFromDate) && (i.DateOfIncident <= incidentsToDate)
                             select i).ToList();

            if (incidents != null)
            {
                foreach (Incident i in incidents)
                {
                    var services = db.ServiceParticipations.Where(p => p.IncidentId == i.ID);
                    foreach(var s in services)
                    {
                        if(User.IsInRole(s.RoleName))
                        {
                            IncidentsViewModel model = new IncidentsViewModel();
                            IncidentType type = dbt.IncidentTypes.SingleOrDefault(t => t.TypeID == i.TypeID);
                            model.ID = i.ID;
                            model.Address = i.Address;
                            model.City = i.City;
                            model.Lat = i.Lat;
                            model.Long = i.Long;
                            model.Type = i.Type;
                            model.IconUrl = type.IconUrl;
                            model.AddDate = i.AddDate;
                            list.Add(model);
                            break;
                        }
                    }
                    
                }
            }
            return View(list);
        }

        public ActionResult SetDates(string startDate, string endDate)
        {
            DateTime incidentsToDate = DateTime.Parse(endDate);
            DateTime incidentsFromDate = DateTime.Parse(startDate);

            List<IncidentsViewModel> list = new List<IncidentsViewModel>();

            var incidents = (from i in db.Incidents
                             where (i.DateOfIncident >= incidentsFromDate) && (i.DateOfIncident <= incidentsToDate)
                             select i).ToList();

            if (incidents != null)
            {
                foreach (Incident i in incidents)
                {
                    var services = db.ServiceParticipations.Where(p => p.IncidentId == i.ID);
                    foreach (var s in services)
                    {
                        if (User.IsInRole(s.RoleName))
                        {
                            IncidentsViewModel model = new IncidentsViewModel();
                            IncidentType type = dbt.IncidentTypes.SingleOrDefault(t => t.TypeID == i.TypeID);
                            model.ID = i.ID;
                            model.Address = i.Address;
                            model.City = i.City;
                            model.Lat = i.Lat;
                            model.Long = i.Long;
                            model.Type = i.Type;
                            model.IconUrl = type.IconUrl;
                            model.AddDate = i.AddDate;
                            list.Add(model);
                            break;
                        }
                    }

                }
            }
            return View("Index", list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Legend.";
            
            return View(db.IncidentTypes.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}