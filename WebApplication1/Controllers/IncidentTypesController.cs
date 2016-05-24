using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Kontroler typu incydentu.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class IncidentTypesController : Controller
    {
        private Context db = new Context();
        /// <summary>
        /// Zapisanie ikony typu incydentu do elementu Blob.
        /// </summary>
        /// <param name="file">Plik ikony.</param>
        /// <returns>Adres do zapisanego pliku.</returns>
        public string saveIconToBlob(HttpPostedFileBase file)
        {
            string azurePath;
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("markericons");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(file.InputStream);

            azurePath = String.Format("http://{0}{1}", blockBlob.Uri.DnsSafeHost, blockBlob.Uri.AbsolutePath);

            return azurePath;
        }

        // GET: IncidentTypes
       /// <summary>
       /// Widok główny z listą typów incydentów.
       /// </summary>
       /// <returns>Widok z listą typów incydentów.</returns>
        public ActionResult Index()
        {
            return View(db.IncidentTypes.ToList());
        }

        // GET: IncidentTypes/Details/5
        /// <summary>
        /// Widok detali typu incydentu.
        /// </summary>
        /// <param name="id">ID typu incydentu.</param>
        /// <returns>Widok detali z modelem typu incydentu.</returns>
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
       /// <summary>
       /// Widok tworzenia typu incydentu.
       /// </summary>
       /// <returns>Widok.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncidentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Tworzenie typu incydentu.
        /// </summary>
        /// <param name="incidentType">Model typu incydentu z danymi z widoku.</param>
        /// <param name="file">Plik ikony typu incydentu do zapisu w elemencie Blob.</param>
        /// <returns>Widok główny w razie powodzenia, widok dodawania typu incydentu z modelem w przeciwnym wypadku.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,Name")] IncidentType incidentType, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    incidentType.IconUrl = saveIconToBlob(file);

                    db.IncidentTypes.Add(incidentType);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(incidentType);
        }

        // GET: IncidentTypes/Edit/5
       /// <summary>
       /// Widok edycji typu incydentu.
       /// </summary>
       /// <param name="id">ID typu incydentu.</param>
       /// <returns>Widok z modelem typu incydentu.</returns>
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
        /// <summary>
        /// Edycja typu incydentu.
        /// </summary>
        /// <param name="incidentType">Model typu incydentu z danymi z widoku.</param>
        /// <returns>Widok główny w razie powodzenia, widok z modelem typu incydentu w przeciwnym wypadku.</returns>
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
       /// <summary>
       /// Widok usuwania typu incydentu.
       /// </summary>
       /// <param name="id">ID typu incydentu.</param>
       /// <returns>Widok z modelem typu incydentu.</returns>
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
        /// <summary>
        /// Usunięcie typu incydentu.
        /// </summary>
        /// <param name="id">ID typu incydentu.</param>
        /// <returns>Widok główny.</returns>
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
