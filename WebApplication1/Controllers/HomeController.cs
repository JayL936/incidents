using System;
using System.Collections.Generic;
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
            //var incidents = new IncidentViewModel
            //{
            //    incident = db.
            //}
            return View(db.Incidents.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            List<IncidentsViewModel> list = new List<IncidentsViewModel>();
            var incidents = db.Incidents;
            foreach(Incident i in incidents)
            {
                IncidentsViewModel model = new IncidentsViewModel();
                IncidentType type = dbt.IncidentTypes.SingleOrDefault(t => t.TypeID == i.TypeID);
                model.ID = i.ID;
                model.Lat = i.Lat;
                model.Long = i.Long;
                model.Type = i.Type;
                model.IconUrl = type.IconUrl;
                list.Add(model);
            }
            return View(list);//db.Incidents.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}