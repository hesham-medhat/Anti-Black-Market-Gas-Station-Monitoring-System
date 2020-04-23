using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSMS.Controllers
{
    public class GasStationController : Controller
    {
        [HttpGet]
        public ActionResult refill(string refillQuantity)
        {
            GasStation gs = (GasStation)Session["user"];
            Entities db = new Entities();
            int quantity = Int32.Parse(refillQuantity);
            GasStation res = db.GasStations.SingleOrDefault(b => b.Id == gs.Id);
            if (refillQuantity.Length > 0)
            {
                RefillHistory history = new RefillHistory();
                history.GasStationID = gs.Id;
                history.Datetime = DateTime.Now;
                history.Quantity = quantity;
                db.RefillHistories.Add(history);
            }
            if ((quantity + gs.EstimatedFuelQuantity) > gs.TotalTankSize)
            {
                UnderInvestigation susbicious = new UnderInvestigation();
                susbicious.GasStationId = gs.Id;
                susbicious.InvestigatorId = "81e5a966-6e05-4d5a-bac4-de47b8eac47b"; //temp till we fiqure out how to assign invistigators to cases
                susbicious.IssueDatetime = DateTime.Now;
                db.UnderInvestigations.Add(susbicious);
            }
            else
            {

                if (res != null)
                {
                    res.EstimatedFuelQuantity = gs.EstimatedFuelQuantity + quantity;
                }
            }
            db.SaveChanges();
            Session["user"] = res;
            return View("../GasStation/AdministrationGasStation", res);
        }

        [HttpGet]
        public ActionResult serveUser(string userName, string serveUserQuantity)
        {
            GasStation gs = (GasStation)Session["user"];
            Entities db = new Entities();
            int quantity = Int32.Parse(serveUserQuantity);

            Citizen res = db.Citizens.SingleOrDefault(b => b.Name == userName);
            GasStation stationres = db.GasStations.SingleOrDefault(b => b.Id == gs.Id);

            if (res != null)
            {
                if (res.Quota >= quantity)
                {

                    if (stationres != null && stationres.EstimatedFuelQuantity >= quantity)
                    {
                        res.Quota = res.Quota - quantity;
                        stationres.EstimatedFuelQuantity = gs.EstimatedFuelQuantity - quantity;
                        db.TransactionHistories.Add(new TransactionHistory()
                        {
                            Quantity = quantity,
                            Datetime = DateTime.Now,
                            GasStationId = stationres.Id,
                            CitizenId = res.Id
                        });
                    }
                }
            }

            db.SaveChanges();
            Session["user"] = stationres;
            return View("../GasStation/AdministrationGasStation", stationres);
        }
    }
}