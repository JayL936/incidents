using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = "Admin, Emergency, City cleaning, Police, Municipal police, Fire department, Other")]
    public class ParticipantsController : Controller
    {
        private Context db = new Context();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Participants
        public ActionResult Index()
        {
            List<int> list = new List<int>();
            var services = db.ServiceParticipations;
            foreach (var s in services)
            {
                if (User.IsInRole(s.RoleName) && s.confirmed == true)
                    if (!list.Contains(s.IncidentId))
                        list.Add(s.IncidentId);
            }
            List<Participant> parts = new List<Participant>();
            foreach (int id in list)
            {
                var participants = db.Participants.Where(p => p.incidentID == id).Include(p => p.ParticipantType);
                foreach (var participant in participants)
                    parts.Add(participant);
            }
            return View(parts);
        }

        // GET: Participants/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            Incident incident = db.Incidents.Find(participant.incidentID);
            var roles = db.ServiceParticipations.Where(i => i.IncidentId == incident.ID);
            foreach (var r in roles)
            {
                if (User.IsInRole(r.RoleName) && r.confirmed == true)
                    return View(participant);
            }

            return RedirectToAction("Index");
        }

        // GET: Participants/Create
        public ActionResult Create(int? id)
        {
            List<int> list = new List<int>();
            var services = db.ServiceParticipations;
            foreach (var s in services)
            {
                if (User.IsInRole(s.RoleName) && s.confirmed == true)
                    if (!list.Contains(s.IncidentId))
                        list.Add(s.IncidentId);
            }

            if (id != null)
                if (!list.Contains((int)id))
                    return RedirectToAction("Index", "Incidents");

            ViewBag.pTypeID = new SelectList(db.ParticipantTypes, "pTypeID", "pTypeName");
            ViewBag.incidentID = new SelectList(list, id);
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PESEL,FirstName,LastName,About,DateOfBirth,pTypeID,incidentID")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                db.Participants.Add(participant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pTypeID = new SelectList(db.ParticipantTypes, "pTypeID", "pTypeName", participant.pTypeID);
            return View(participant);
        }

        // GET: Participants/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            Incident incident = db.Incidents.Find(participant.incidentID);
            List<int> list = new List<int>();
            var services = db.ServiceParticipations;
            foreach (var s in services)
            {
                if (User.IsInRole(s.RoleName))
                    if (!list.Contains(s.IncidentId))
                        list.Add(s.IncidentId);
            }
            var roles = db.ServiceParticipations.Where(i => i.IncidentId == incident.ID);
            foreach (var r in roles)
            {
                if (User.IsInRole(r.RoleName) && r.confirmed == true)
                {
                    ViewBag.incidentID = new SelectList(list, id);
                    ViewBag.pTypeID = new SelectList(db.ParticipantTypes, "pTypeID", "pTypeName", participant.pTypeID);
                    return View(participant);
                }
            }

            return RedirectToAction("Index");

        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PESEL,FirstName,LastName,About,DateOfBirth,pTypeID,incidentID")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pTypeID = new SelectList(db.ParticipantTypes, "pTypeID", "pTypeName", participant.pTypeID);
            return View(participant);
        }

        // GET: Participants/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            Incident incident = db.Incidents.Find(participant.incidentID);
            var roles = db.ServiceParticipations.Where(i => i.IncidentId == incident.ID);
            foreach (var r in roles)
            {
                if (User.IsInRole(r.RoleName) && r.confirmed == true)
                    return View(participant);
            }

            return RedirectToAction("Index");
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Participant participant = db.Participants.Find(id);
            db.Participants.Remove(participant);
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
