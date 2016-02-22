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
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            Incident newIncident = new Incident();
            newIncident.AddDate = DateTime.Today.Date;
            newIncident.DateOfIncident = DateTime.Today.Date;
            newIncident.TimeOfIncident = DateTime.Today.TimeOfDay;
            List<SelectListItem> li = new List<SelectListItem>();
            var query = from types in db.IncidentTypes select types;
            foreach(var type in query)
            {
                li.Add(new SelectListItem { Text = type.Name, Value = type.Name });
            }
            ViewData["Types"] = li;

            return this.View(newIncident);
        }

        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult CreateIncident(string JsonStr)
        {
            Incident newIncident = new Incident();
            newIncident.Address = JsonStr;
        //   RedirectToAction("Create");
           return View("Create", newIncident);
            
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AddDate,DateOfIncident,TimeOfIncident,Type,About,Lat,Long,Address,City,ZipCode")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(incident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incident);
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
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AddDate,DateOfIncident,TimeOfIncident,Type,About,Lat,Long,Address,City,ZipCode")] Incident incident, string Types)
        {
            if (ModelState.IsValid)
            {
               // incident.Type = Types;
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incident);
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
            return View(incident);
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
    }
}
