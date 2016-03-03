using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class IncidentTypesController : Controller
    {
        private Context db = new Context();

        public void saveIconToBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("markericons");
            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }

        // GET: IncidentTypes
        public ActionResult Index()
        {
            return View(db.IncidentTypes.ToList());
        }

        // GET: IncidentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentType incidentType = db.IncidentTypes.Find(id);
            if (incidentType == null)
            {
                return HttpNotFound();
            }
            return View(incidentType);
        }

        // GET: IncidentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncidentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,Name")] IncidentType incidentType)
        {
            if (ModelState.IsValid)
            {
                db.IncidentTypes.Add(incidentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incidentType);
        }

        // GET: IncidentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentType incidentType = db.IncidentTypes.Find(id);
            if (incidentType == null)
            {
                return HttpNotFound();
            }
            return View(incidentType);
        }

        // POST: IncidentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,Name")] IncidentType incidentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incidentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incidentType);
        }

        // GET: IncidentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentType incidentType = db.IncidentTypes.Find(id);
            if (incidentType == null)
            {
                return HttpNotFound();
            }
            return View(incidentType);
        }

        // POST: IncidentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentType incidentType = db.IncidentTypes.Find(id);
            db.IncidentTypes.Remove(incidentType);
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
