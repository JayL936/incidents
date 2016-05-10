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
    public class ServiceParticipationsController : Controller
    {
        private Context db = new Context();

        // GET: ServiceParticipations
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
