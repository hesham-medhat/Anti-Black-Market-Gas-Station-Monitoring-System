using System;
using System.Linq;

namespace GSMS.Services
{
    public class UserService
    {
        private Entities db = new Entities();

        internal User getUser(string Id)
        {
            return db.Users.SingleOrDefault(b => b.Id == Id);
        }

        internal GasStation getStation(string Id)
        {
            return db.GasStations.SingleOrDefault(b => b.Id == Id);
        }

        internal Citizen getCitizen(string Id)
        {
            return db.Citizens.SingleOrDefault(b => b.Id == Id);
        }

        internal void CreateGasStation(string Email, string Name)
        {
            User user = db.Users.SingleOrDefault(b => b.Email == Email);
            GasStation gasStation = new GasStation() { 
                Id = user.Id,
                Name = Name,
                EstimatedFuelQuantity = 0,
                TotalTankSize = 1000
            };
            db.GasStations.Add(gasStation);
            db.SaveChanges();
        }

        internal void CreateInvestigator(string Id, string Name)
        {
            throw new NotImplementedException();
        }

        internal void CreateCitizen(string Email, string Name)
        {
            User user = db.Users.SingleOrDefault(b => b.Email == Email);
            Citizen citizen = new Citizen() { 
                Id = user.Id,
                Name = Name,
                Quota = 200
            };
            db.Citizens.Add(citizen);
            db.SaveChanges();
        }
    }
}