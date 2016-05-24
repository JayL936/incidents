using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Kontroler incydentów. Dostępny dla wszystkich oprócz roli Viewer.
    /// </summary>
    [Authorize(Roles="Admin, Emergency, City cleaning, Police, Municipal police, Fire department, Other")]
    public class IncidentsController : Controller
    {
        private Context db = new Context();
        private Context dbt = new Context();
        private ApplicationDbContext appDb = new ApplicationDbContext();

        // GET: Incidents
        /// <summary>
        /// Widok główny z listą incydentów.
        /// </summary>
        /// <returns>Widok z listą incydentów.</returns>
        public ActionResult Index()
        {
            List<IncidentsViewModel> list = new List<IncidentsViewModel>();
            var incidents = db.Incidents;
            if (incidents != null)
            {
                foreach (Incident i in incidents)
                {
                    var services = db.ServiceParticipations.Where(p => p.IncidentId == i.ID);
                    foreach (var s in services)
                    {
                        if (User.IsInRole(s.RoleName) || User.IsInRole("Viewer"))
                        {
                            IncidentsViewModel model = new IncidentsViewModel();
                            IncidentType type = dbt.IncidentTypes.SingleOrDefault(t => t.TypeID == i.TypeID);
                            model.ID = i.ID;
                            model.AddDate = i.AddDate;
                            model.DateOfIncident = i.DateOfIncident;
                            model.TimeOfIncident = i.TimeOfIncident;
                            model.Address = i.Address;
                            model.City = i.City;
                            model.ZipCode = i.ZipCode;
                            model.Lat = i.Lat;
                            model.Long = i.Long;
                            model.Type = i.Type;
                            model.IconUrl = type.IconUrl;
                            model.confirmed = s.confirmed;
                            list.Add(model);
                            break;
                        }
                    }

                }
            }
            return View(list);
        }

        // GET: Incidents/Details/5
        /// <summary>
        /// Widok detali incydentu.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Widok detali incydentu lub listę incydentów przy braku uprawnień.</returns>
        public ActionResult Details(int? id)
        {
            bool authorized = false;
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
            viewModel.ZipCode = incident.ZipCode;
            viewModel.Participants = db.Participants.Where(p => p.incidentID == incident.ID);

            var incidentRoles = db.ServiceParticipations.Where(s => s.IncidentId == id); 
            //(from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach (RoleViewModel r in viewModel.Roles)
            {
                foreach (var i in incidentRoles)
                {
                    if (r.RoleId == i.RoleId)
                    {
                        r.Selected = true;
                        if (User.IsInRole(r.RoleName))
                        {
                            authorized = true;
                            viewModel.confirmed = i.confirmed;
                        }
                    }
                }
            }

            if (authorized)  
                return View(viewModel);
            else
                return RedirectToAction("Index");
        }

        /// <summary>
        /// Potwierdzenie incydentu.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Przekierowanie do akcji widoku głównego.</returns>
        public ActionResult ConfirmIncident(int id)
        {
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            var part = db.ServiceParticipations.Where(p => p.IncidentId == id);
            foreach(var p in part)
            {
                if (User.IsInRole(p.RoleName))
                {
                    db.ServiceParticipations.Remove(p);
                    ServiceParticipation sp = new ServiceParticipation();
                    sp.IncidentId = id;
                    sp.confirmed = true;
                    sp.RoleId = p.RoleId;
                    sp.RoleName = p.RoleName;
                    db.ServiceParticipations.Add(sp);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Zaprzeczenie udzialu w incydencie.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Przekierowanie do akcji widoku głównego incydentów.</returns>
        public ActionResult DenyIncident(int id)
        {
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            var part = db.ServiceParticipations.Where(p => p.IncidentId == id);
            foreach (var p in part)
            {
                if (User.IsInRole(p.RoleName))
                {
                    db.ServiceParticipations.Remove(p);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Incidents/Create
        /// <summary>
        /// Widok tworzenia incydentu.
        /// </summary>
        /// <returns>Widok tworzenia incydentu z modelem uzupełnionym datami oraz listą typów incydentów.</returns>
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

        /// <summary>
        /// Akcja kreowania incydentu z poziomu strony głównej.
        /// </summary>
        /// <param name="address">Adres dodania incydentu.</param>
        /// <returns>Widok kreowania incydentu z uzupełnionym adresem i datami.</returns>
        [HttpPost]
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
        /// <summary>
        /// Tworzenie incydentu.
        /// </summary>
        /// <param name="incidentView">Model incydentu z widoku.</param>
        /// <returns>Widok główny lub widok tworzenia incydentu w razie błędów.</returns>
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
                foreach (RoleViewModel r in incidentView.Roles)
                {
                    if (r.Selected == true)
                    {
                        ServiceParticipation part = new ServiceParticipation();
                        var role = appDb.Roles.SingleOrDefault(o => o.Id == r.RoleId);
                        part.RoleName = role.Name;
                        part.RoleId = r.RoleId;
                        part.IncidentId = incidentView.ID;
                        if (User.IsInRole(r.RoleName))
                            part.confirmed = true;
                        else
                            part.confirmed = false;
                        db.ServiceParticipations.Add(part);   
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<SelectListItem> li = new List<SelectListItem>();
            var allTypes = from types in db.IncidentTypes select types;

            foreach (var type in allTypes)
            {
                li.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }

            ViewData["Types"] = li;

            return View(incidentView);
        }

        // GET: Incidents/Edit/5
        /// <summary>
        /// Widok edycji incydentów.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Widok edycji incydentu lub listy incydentów przy braku uprawnień.</returns>
        public ActionResult Edit(int? id)
        {
            bool authorized = false;
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
            var incidentRoles = db.ServiceParticipations.Where(s => s.IncidentId == id);
           //     (from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach (RoleViewModel r in viewModel.Roles)
            {
                foreach (var i in incidentRoles)
                {
                    if (r.RoleId == i.RoleId)
                    {
                        r.Selected = true;
                        if (User.IsInRole(r.RoleName) && i.confirmed == true)
                            authorized = true;
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

            if (authorized)
                return View(viewModel);
            else
                return RedirectToAction("Index");
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edycja incydentu.
        /// </summary>
        /// <param name="incidentView">Model incydentu z widoku.</param>
        /// <returns>Widok listy incydentów lub widok tworzenia incydentu w przypadku błędów.</returns>
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
                foreach (var s in services)
                {
                    db.ServiceParticipations.Remove(s);
                }
                foreach (RoleViewModel r in incidentView.Roles)
                {
                    if (r.Selected == true)
                    {
                        ServiceParticipation part = new ServiceParticipation();
                        var role = appDb.Roles.SingleOrDefault(o => o.Id == r.RoleId);
                        part.RoleName = role.Name;
                        part.RoleId = r.RoleId;
                        part.IncidentId = incidentView.ID;
                        if (User.IsInRole(r.RoleName))
                            part.confirmed = true;
                        else
                            part.confirmed = false;
                        db.ServiceParticipations.Add(part);
                        //db.SaveChanges();
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incidentView);
        }

        // GET: Incidents/Delete/5
        /// <summary>
        /// Widok usuwania incydentów.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Widok usuwania incydentów lub widok listy incydentów w przypadku braku uprawnień.</returns>
        public ActionResult Delete(int? id)
        {
            bool authorized = false;
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

            var incidentRoles = db.ServiceParticipations.Where(s => s.IncidentId == id); 
            //(from roles in db.ServiceParticipations where roles.IncidentId == id select roles.RoleId);
            viewModel.Roles = GetRoles();
            foreach (RoleViewModel r in viewModel.Roles)
            {
                foreach (var i in incidentRoles)
                {
                    if (r.RoleId == i.RoleId)
                    {
                        r.Selected = true;
                        if (User.IsInRole(r.RoleName) && i.confirmed == true)
                            authorized = true;
                    }
                }
            }

            if (authorized)
                return View(viewModel);
            else
                return RedirectToAction("Index");
        }

        // POST: Incidents/Delete/5
        /// <summary>
        /// Usunięcie incydentu.
        /// </summary>
        /// <param name="id">ID incydentu.</param>
        /// <returns>Przekierowanie do widoku głównego.</returns>
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

        /// <summary>
        /// Pobranie ról służb.
        /// </summary>
        /// <returns>Listę dostępnych ról poza administratrem i widzem.</returns>
        public List<RoleViewModel> GetRoles()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            var roles = appDb.Roles;
            if (roles != null)
            {
                foreach (IdentityRole r in roles)
                {
                    if (r.Name != "Admin" && r.Name != "Viewer")
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
