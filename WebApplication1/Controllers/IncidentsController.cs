using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IncidentsController : Controller
    {
        private Context db = new Context();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // GET: Incidents
        public ActionResult Index()
        {
            return View(db.Incidents.ToList());
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);

            if (incident == null)
            {
                return HttpNotFound();
            }

            IncidentsViewModel viewModel = new IncidentsViewModel();
            viewModel.ID = incident.ID;
            viewModel.About = incident.About;
            viewModel.AddDate = incident.AddDate;
            viewModel.Address = incident.Address;
            viewModel.City = incident.City;
            viewModel.DateOfIncident = incident.DateOfIncident;
            viewModel.Lat = incident.Lat;
            viewModel.Long = incident.Long;
            viewModel.TimeOfIncident = incident.TimeOfIncident;
            viewModel.Type = incident.Type;

            var incidentRoles = (from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach (RoleViewModel r in viewModel.Roles)
            {
                foreach (var i in incidentRoles)
                {
                    if (r.RoleId == i)
                    {
                        r.Selected = true;
                    }
                }
            }

            return View(viewModel);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            IncidentsViewModel newIncident = new IncidentsViewModel();
            newIncident.AddDate = DateTime.Today.Date;
            newIncident.DateOfIncident = DateTime.Today.Date;
            newIncident.TimeOfIncident = DateTime.Today.TimeOfDay;
            newIncident.Roles = GetRoles();

            List<SelectListItem> li = new List<SelectListItem>();
            var query = from types in db.IncidentTypes select types;

            foreach (var type in query)
            {
                li.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }
            ViewData["Types"] = li;

            return this.View(newIncident);
        }

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult CreateIncident(string address)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            var query = from types in db.IncidentTypes select types;

            foreach (var type in query)
            {
                li.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }

            ViewData["Types"] = li;

            IncidentsViewModel newIncident = new IncidentsViewModel();
            newIncident.AddDate = DateTime.Today.Date;
            newIncident.DateOfIncident = DateTime.Today.Date;
            newIncident.TimeOfIncident = DateTime.Today.TimeOfDay;
            newIncident.Roles = GetRoles();

            if (address != "")
            {
                char[] separators = { ',' };
                string[] fullAddress = address.Split(separators);


                if (fullAddress[1].Any(char.IsDigit))
                {
                    newIncident.ZipCode = fullAddress[1].Substring(0, 7).Trim();
                    newIncident.City = fullAddress[1].Substring(8).Trim();
                }
                else
                {
                    newIncident.ZipCode = "XX-XXX";
                    newIncident.City = fullAddress[1].Trim();
                }

                newIncident.Address = fullAddress[0].Trim();
            }

            return View("Create", newIncident);
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AddDate,DateOfIncident,TimeOfIncident,Type,About,Lat,Long,Address,City,ZipCode,Roles")] IncidentsViewModel incidentView)
        {
            if (ModelState.IsValid)
            {
                Incident incident = new Incident();
                var query = (from t in db.IncidentTypes where t.Name.Equals(incidentView.Type) select t.TypeID).FirstOrDefault();
                incident.TypeID = query;
                incident.About = incidentView.About;
                incident.AddDate = incidentView.AddDate;
                incident.Address = incidentView.Address;
                incident.City = incidentView.City;
                incident.DateOfIncident = incidentView.DateOfIncident;
                incident.Lat = incidentView.Lat;
                incident.Long = incidentView.Long;
                incident.TimeOfIncident = incidentView.TimeOfIncident;
                incident.Type = incidentView.Type;
                incident.ZipCode = incidentView.ZipCode;
                db.Incidents.Add(incident);          
                foreach(RoleViewModel r in incidentView.Roles)
                {
                    if(r.Selected == true)
                    {
                        ServiceParticipation part = new ServiceParticipation();
                        part.RoleId = r.RoleId;
                        part.RoleName = r.RoleName;
                        part.IncidentId = incidentView.ID;
                        db.ServiceParticipations.Add(part);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incidentView);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            incident.Lat.ToString().Replace(",", ".");
            incident.Long.ToString().Replace(",", ".");
            IncidentsViewModel viewModel = new IncidentsViewModel();
            viewModel.ID = incident.ID;
            viewModel.About = incident.About;
            viewModel.AddDate = incident.AddDate;
            viewModel.Address = incident.Address;
            viewModel.City = incident.City;
            viewModel.DateOfIncident = incident.DateOfIncident;
            viewModel.Lat = incident.Lat;
            viewModel.Long = incident.Long;
            viewModel.TimeOfIncident = incident.TimeOfIncident;
            viewModel.Type = incident.Type;
            var incidentRoles = (from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach(RoleViewModel r in viewModel.Roles)
            {
                foreach(var i in incidentRoles)
                {
                    if (r.RoleId == i)
                    {
                        r.Selected = true;
                    }
                }
            }
            if (incident == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> li = new List<SelectListItem>();
            var query = from types in db.IncidentTypes select types;
            foreach (var type in query)
            {
                li.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }

            ViewData["Types"] = li;
            return View(viewModel);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AddDate,DateOfIncident,TimeOfIncident,Type,About,Lat,Long,Address,City,ZipCode,Roles")] IncidentsViewModel incidentView)
        {
            if (ModelState.IsValid)
            {
                Incident incident = new Incident();
                var query = (from t in db.IncidentTypes where t.Name.Equals(incidentView.Type) select t.TypeID).FirstOrDefault();
                incident.ID = incidentView.ID;
                incident.TypeID = query;
                incident.About = incidentView.About;
                incident.AddDate = incidentView.AddDate;
                incident.Address = incidentView.Address;
                incident.City = incidentView.City;
                incident.DateOfIncident = incidentView.DateOfIncident;
                incident.Lat = incidentView.Lat;
                incident.Long = incidentView.Long;
                incident.TimeOfIncident = incidentView.TimeOfIncident;
                incident.Type = incidentView.Type;
                incident.ZipCode = incidentView.ZipCode;
                db.Entry(incident).State = EntityState.Modified;
                var services = db.ServiceParticipations.Where(s => s.IncidentId == incidentView.ID);
                foreach(var s in services)
                {
                    db.ServiceParticipations.Remove(s);
                }
                foreach (RoleViewModel r in incidentView.Roles)
                {
                    if (r.Selected == true)
                    {
                        ServiceParticipation part = new ServiceParticipation();
                        part.RoleName = r.RoleName;
                        part.RoleId = r.RoleId;
                        part.IncidentId = incidentView.ID;
                       
                        db.ServiceParticipations.Add(part);
                    }
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incidentView);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);

            if (incident == null)
            {
                return HttpNotFound();
            }

            IncidentsViewModel viewModel = new IncidentsViewModel();
            viewModel.ID = incident.ID;
            viewModel.About = incident.About;
            viewModel.AddDate = incident.AddDate;
            viewModel.Address = incident.Address;
            viewModel.City = incident.City;
            viewModel.DateOfIncident = incident.DateOfIncident;
            viewModel.Lat = incident.Lat;
            viewModel.Long = incident.Long;
            viewModel.TimeOfIncident = incident.TimeOfIncident;
            viewModel.Type = incident.Type;

            var incidentRoles = (from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach (RoleViewModel r in viewModel.Roles)
            {
                foreach (var i in incidentRoles)
                {
                    if (r.RoleId == i)
                    {
                        r.Selected = true;
                    }
                }
            }

            return View(viewModel);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<RoleViewModel> GetRoles()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            var roles = appDb.Roles;
            if (roles != null)
            {
                foreach (IdentityRole r in roles)
                {
                    if (r.Name != "Admin")
                    {
                        RoleViewModel model = new RoleViewModel();
                        model.RoleId = r.Id;
                        model.RoleName = r.Name;
                        list.Add(model);
                    }
                }
            }
            return list;
        }
    }
}
