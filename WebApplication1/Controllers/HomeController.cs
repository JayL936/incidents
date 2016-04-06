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
            var model = (from i in db.Incidents
                         join t in db.IncidentTypes on i.TypeID equals t.TypeID
                         select new IncidentsViewModel {
                                 i.IncidentType.Name,
                                 i.Type
                             });
            return View(db.Incidents.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}