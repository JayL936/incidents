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
    /// <summary>
    /// Kontroler uczestników. Dostępny dla wszystkich poza rolą Viewer.
    /// </summary>
    [Authorize(Roles = "Admin, Emergency, City cleaning, Police, Municipal police, Fire department, Other")]
    public class ParticipantsController : Controller
    {
        private Context db = new Context();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Participants
        /// <summary>
        /// Widok listy uczestników.
        /// </summary>
        /// <returns>Widok z listą uczestników.</returns>
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
        /// <summary>
        /// Widok detali uczestnika.
        /// </summary>
        /// <param name="id">ID uczestnika.</param>
        /// <returns>Widok detali uczestnika lub przekierowanie do widoku głównego przy braku uprawnień.</returns>
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
        /// <summary>
        /// Widok tworzenia uczestnika.
        /// </summary>
        /// <param name="id">ID incydentu, dla którego tworzy się uczestnika.</param>
        /// <returns>Widok Create lub przekierowanie do listy incydentów w razie podania ID, dla którego nie ma incydentu.</returns>
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
        /// <summary>
        /// Tworzenie uczestnika.
        /// </summary>
        /// <param name="participant">Model uczestnika z danymi z widoku.</param>
        /// <returns>Widok główny w razie powodzenia, widok z danymi uczestnika, jeśli były błędy walidacji.</returns>
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
        /// <summary>
        /// Widok edycji uczestnika.
        /// </summary>
        /// <param name="id">ID uczestnika</param>
        /// <returns>Widok z modelem uczestnika lub przekierowanie do widoku głównego.</returns>
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
        /// <summary>
        /// Zapis edycji uczestnika.
        /// </summary>
        /// <param name="participant">Model uczestnika z danymi z widoku.</param>
        /// <returns>Widok główny w razie powodzenia, widok edycji z modelem uczestnika przy błędach walidacji.</returns>
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
        /// <summary>
        /// Widok usuwania uczestnika.
        /// </summary>
        /// <param name="id">ID uczestnika.</param>
        /// <returns>Widok z modelem uczestnika lub widok główny przy braku uprawnień.</returns>
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
        /// <summary>
        /// Usuwanie uczestnika.
        /// </summary>
        /// <param name="id">ID uczestnika.</param>
        /// <returns>Widok główny.</returns>
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
