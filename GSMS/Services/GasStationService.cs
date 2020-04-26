using System;
using System.Linq;

namespace GSMS.Services
{
    public class GasStationService
    {

        private Entities db = new Entities();
        internal void SubmitRefill(int refillQuantity, string Id, bool grant)
        {
            GasStation res = db.GasStations.SingleOrDefault(b => b.Id == Id);
            if (refillQuantity > 0)
            {
                RefillHistory history = new RefillHistory()
                {
                    GasStationID = Id,
                    Datetime = DateTime.Now,
                    Quantity = refillQuantity,
                    Granted = grant
                };
                db.RefillHistories.Add(history);
            }
            if (!grant)
            {
                UnderInvestigation susbicious = new UnderInvestigation()
                {
                    GasStationId = Id,
                    InvestigatorId = "81e5a966-6e05-4d5a-bac4-de47b8eac47b", //Should be Emitted
                    IssueDatetime = DateTime.Now
                };
                db.UnderInvestigations.Add(susbicious);
            }
            else
            {
                if (res != null)
                {
                    res.EstimatedFuelQuantity += refillQuantity;
                }
            }
            db.SaveChanges();
        }

        internal void SubmitServeUSer(int serveUserQuantity, string userName, string Id)
        {
            Citizen res = db.Citizens.SingleOrDefault(b => b.Name == userName);
            GasStation stationres = db.GasStations.SingleOrDefault(b => b.Id == Id);
            if (res != null
                && stationres != null
                && res.Quota >= serveUserQuantity
                && stationres.EstimatedFuelQuantity >= serveUserQuantity)
            {
                res.Quota -= serveUserQuantity;
                stationres.EstimatedFuelQuantity -= serveUserQuantity;
                db.TransactionHistories.Add(new TransactionHistory()
                {
                    Quantity = serveUserQuantity,
                    Datetime = DateTime.Now,
                    GasStationId = stationres.Id,
                    CitizenId = res.Id
                });
                db.SaveChanges();
            }
        }
    }
}