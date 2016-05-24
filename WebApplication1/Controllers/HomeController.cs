using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Kontroler strony domowej.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// </summary>
        Context db = new Context();
        /// <summary>
        /// </summary>
        Context dbt = new Context();

        /// <summary>
        /// Widok strony głównej z listą incydentów i opcjami filtrowania incydentów z danego okresu.
        /// </summary>
        /// <returns>
        /// Widok strony głównej z listą incydentów.
        /// </returns>
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
                        if(User.IsInRole(s.RoleName) || User.IsInRole("Viewer"))
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
                            model.DateOfIncident = i.DateOfIncident;
                            list.Add(model);
                            break;
                        }
                    }
                    
                }
            }
            return View(list);
        }

        /// <summary>
        /// Filtrowanie incydentów zgodnie z określonym przedziałem dat.
        /// </summary>
        /// <param name="startDate">Data początkowa.</param>
        /// <param name="endDate">Data końcowa.</param>
        /// <returns>
        /// Widok strony głównej z listą incydentów spomiędzy podanego przedziału czasu.
        /// </returns>
        public ActionResult SetDates(string startDate, string endDate)
        {
            DateTime incidentsToDate = DateTime.ParseExact(endDate, "dd/M/yyyy", CultureInfo.InvariantCulture );
            DateTime incidentsFromDate = DateTime.ParseExact(startDate, "dd/M/yyyy", CultureInfo.InvariantCulture);

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
                            model.DateOfIncident = i.DateOfIncident;
                            list.Add(model);
                            break;
                        }
                    }

                }
            }
            return View("Index", list);
        }

        /// <summary>
        /// Widok legendy wskazującej ikony odpowiadające typom incydentów.
        /// </summary>
        /// <returns>
        /// Widok z listą typów incydentów.
        /// </returns>
        public ActionResult About()
        {
            ViewBag.Message = "Legend.";
            
            return View(db.IncidentTypes.ToList());
        }

        /// <summary>
        /// Contacts this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}