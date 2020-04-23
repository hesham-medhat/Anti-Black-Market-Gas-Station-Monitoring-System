using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GSMS;

namespace GSMS.Controllers
{
    public class GasStationsController : Controller
    {
        private Entities db = new Entities();

        // GET: GasStations
        public ActionResult Index()
        {
            var gasStations = db.GasStations.Include(g => g.FlaggedStation).Include(g => g.User).Include(g => g.UnderInvestigation);
            return View(gasStations.ToList());
        }

        // GET: GasStations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            return View(gasStation);
        }

        // GET: GasStations/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.FlaggedStations, "GasStationId", "InvestigatorId");
            ViewBag.Id = new SelectList(db.Users, "Id", "Email");
            ViewBag.Id = new SelectList(db.UnderInvestigations, "GasStationId", "InvestigatorId");
            return View();
        }

        // POST: GasStations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TotalTankSize,EstimatedFuelQuantity,Id,Location,Name")] GasStation gasStation)
        {
            if (ModelState.IsValid)
            {
                db.GasStations.Add(gasStation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.FlaggedStations, "GasStationId", "InvestigatorId", gasStation.Id);
            ViewBag.Id = new SelectList(db.Users, "Id", "Email", gasStation.Id);
            ViewBag.Id = new SelectList(db.UnderInvestigations, "GasStationId", "InvestigatorId", gasStation.Id);
            return View(gasStation);
        }

        // GET: GasStations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.FlaggedStations, "GasStationId", "InvestigatorId", gasStation.Id);
            ViewBag.Id = new SelectList(db.Users, "Id", "Email", gasStation.Id);
            ViewBag.Id = new SelectList(db.UnderInvestigations, "GasStationId", "InvestigatorId", gasStation.Id);
            return View(gasStation);
        }

        // POST: GasStations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TotalTankSize,EstimatedFuelQuantity,Id,Location,Name")] GasStation gasStation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gasStation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.FlaggedStations, "GasStationId", "InvestigatorId", gasStation.Id);
            ViewBag.Id = new SelectList(db.Users, "Id", "Email", gasStation.Id);
            ViewBag.Id = new SelectList(db.UnderInvestigations, "GasStationId", "InvestigatorId", gasStation.Id);
            return View(gasStation);
        }

        // GET: GasStations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            return View(gasStation);
        }

        // POST: GasStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GasStation gasStation = db.GasStations.Find(id);
            db.GasStations.Remove(gasStation);
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
