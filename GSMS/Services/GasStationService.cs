using System;
using System.Linq;

namespace GSMS.Services
{
    public class GasStationService
    {

        private Entities db = new Entities();
        public void SubmitRefill(int refillQuantity, string Id, bool grant)
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
                if (db.UnderInvestigations.Find(Id) == null)
                {
                    Random random = new Random();
                    int toSkip = random.Next(0, db.Investigators.Count());
                    Investigator randomInvestigator = db.Investigators.OrderBy(b => Guid.NewGuid()).Skip(toSkip).Take(1).First();
                    UnderInvestigation susbicious = new UnderInvestigation()
                    {
                        GasStationId = Id,
                        InvestigatorId = randomInvestigator.Id,
                        IssueDatetime = DateTime.Now
                    };
                    db.UnderInvestigations.Add(susbicious);
                }
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

        public void SubmitServeUSer(int serveUserQuantity, string userName, string Id)
        {
            Citizen res = db.Citizens.SingleOrDefault(b => b.Name == userName);
            GasStation stationres = db.GasStations.SingleOrDefault(b => b.Id == Id);
            if (res != null
                && stationres != null
                && res.Quota >= serveUserQuantity
                && stationres.EstimatedFuelQuantity >= serveUserQuantity
                && serveUserQuantity > 0)
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