using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        Context db = new Context();
        Context dbt = new Context();

        public ActionResult Index()
        {
            List<IncidentsViewModel> list = new List<IncidentsViewModel>();
            var incidents = db.Incidents;
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
                            list.Add(model);
                            break;
                        }
                    }
                    
                }
            }
            return View(list);
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