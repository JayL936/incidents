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
    /// Kontroler typów uczestników. Dostępne dla wszystkich poza rolą Viewer.
    /// </summary>
    [Authorize(Roles = "Admin, Emergency, City cleaning, Police, Municipal police, Fire department, Other")]
    public class ParticipantTypesController : Controller
    {
        private Context db = new Context();

        // GET: ParticipantTypes
        /// <summary>
        /// Widok główny.
        /// </summary>
        /// <returns>Widok z listą typów uczestnictwa.</returns>
        public ActionResult Index()
        {
            return View(db.ParticipantTypes.ToList());
        }

        // GET: ParticipantTypes/Details/5
        /// <summary>
        /// Widok detali typu uczestnictwa.
        /// </summary>
        /// <param name="id">ID typu uczestnictwa.</param>
        /// <returns>Zwraca widok z modelem typu uczestnictwa lub odpowiednie błędy przy braku typu o danym ID.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantType participantType = db.ParticipantTypes.Find(id);
            if (participantType == null)
            {
                return HttpNotFound();
            }
            return View(participantType);
        }

        // GET: ParticipantTypes/Create
        /// <summary>
        /// Widok tworzenia typu uczestnictwa.
        /// </summary>
        /// <returns>Pusty widok.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParticipantTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Zapis utworzonego typu uczestnictwa.
        /// </summary>
        /// <param name="participantType">Model typu uczestnictwa pobrany z widoku.</param>
        /// <returns>Widok z modelem typu uczestnictwa.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pTypeID,pTypeName")] ParticipantType participantType)
        {
            if (ModelState.IsValid)
            {
                db.ParticipantTypes.Add(participantType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(participantType);
        }

        // GET: ParticipantTypes/Edit/5
        /// <summary>
        /// Widok edycji typu uczestnictwa.
        /// </summary>
        /// <param name="id">ID typu uczestnictwa.</param>
        /// <returns>Widok z modelem typu uczestnictwa.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantType participantType = db.ParticipantTypes.Find(id);
            if (participantType == null)
            {
                return HttpNotFound();
            }
            return View(participantType);
        }

        // POST: ParticipantTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Zapis edytowanego typu uczestnictwa.
        /// </summary>
        /// <param name="participantType">Dane modelu typu uczestnictwa pobrane z widoku.</param>
        /// <returns>Widok z modelem uczestnictwa.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pTypeID,pTypeName")] ParticipantType participantType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participantType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(participantType);
        }

        // GET: ParticipantTypes/Delete/5
        /// <summary>
        /// Usunięcie wybranego typu uczestnictwa.
        /// </summary>
        /// <param name="id">ID typu uczestnictwa.</param>
        /// <returns>Widok z modelem typu uczestnictwa.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantType participantType = db.ParticipantTypes.Find(id);
            if (participantType == null)
            {
                return HttpNotFound();
            }
            return View(participantType);
        }

        // POST: ParticipantTypes/Delete/5
        /// <summary>
        /// Potwierdzenie usunięcia typu uczestnictwa.
        /// </summary>
        /// <param name="id">ID typu uczestnictwa.</param>
        /// <returns>Przekierowanie do akcji głównej.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParticipantType participantType = db.ParticipantTypes.Find(id);
            db.ParticipantTypes.Remove(participantType);
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
