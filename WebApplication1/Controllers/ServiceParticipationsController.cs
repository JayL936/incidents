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
    /// Kontroler uczestnictwa w incydencie, dostępny dla wszystkich poza rolą Viewer
    /// </summary>
    [Authorize(Roles = "Admin, Emergency, City cleaning, Police, Municipal police, Fire department, Other")]
    public class ServiceParticipationsController : Controller
    {
        private Context db = new Context();

        // GET: ServiceParticipations
        /// <summary>
        /// Wyświetlenie strony głównej z listą uczestnicw
        /// </summary>
        /// <returns>Widok z listą uczestnictw</returns>
        public ActionResult Index()
        {
            List<ServiceParticipation> li = new List<ServiceParticipation>();
            var serviceParticipations = db.ServiceParticipations.Include(s => s.Incident);
            foreach(var sp in serviceParticipations)
            {
                if(User.IsInRole(sp.RoleName) && sp.confirmed == true)
                {
                    li.Add(sp);
                }
            }
            return View(li);
        }

        // GET: ServiceParticipations/Details/5
        /// <summary>
        /// Widok detali uczestnictwa
        /// </summary>
        /// <param name="id">ID incydentu</param>
        /// <returns>Widok detali uczestnictwa lub widok główny przy braku uprawnień</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceParticipation = from sp in db.ServiceParticipations where sp.IncidentId == id select sp;
            if (serviceParticipation == null)
            {
                return HttpNotFound();
            }
            foreach(var sp in serviceParticipation)
            {
                if (User.IsInRole(sp.RoleName) && sp.confirmed == true)
                    return View(sp);
            }
            return View("Index");
        }

        // GET: ServiceParticipations/Edit/5
        /// <summary>
        /// Widok edycji danych uczestnictwa. Edytowalne tylko dodatkowe informacje o uczestnictwie
        /// </summary>
        /// <param name="id">ID incydentu</param>
        /// <returns>Widok dodatkowych informacji uczestnictwa lub widok główny przy braku uprawnień</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var serviceParticipation = from sp in db.ServiceParticipations where sp.IncidentId == id select sp;
            if (serviceParticipation == null)
            {
                return HttpNotFound();
            }
            foreach (var sp in serviceParticipation)
            {
                if (User.IsInRole(sp.RoleName) && sp.confirmed == true)
                    return View(sp);
            }
            return View("Index");
        }

        // POST: ServiceParticipations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Zapis edytowanych danych uczestnictwa
        /// </summary>
        /// <param name="serviceParticipation">Dane uczestnictwa</param>
        /// <returns>Widok wraz z modelem uczestnictwa</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,RoleId,RoleName,confirmed,AboutParticipation")] ServiceParticipation serviceParticipation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceParticipation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceParticipation);
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
