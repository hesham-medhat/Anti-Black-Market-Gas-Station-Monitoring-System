using System;
using System.Linq;

namespace GSMS.Services
{
    public class UserService
    {
        private Entities db = new Entities();

        public User getUser(string Id)
        {
            return db.Users.SingleOrDefault(b => b.Id == Id);
        }

        public GasStation getStation(string Id)
        {
            return db.GasStations.SingleOrDefault(b => b.Id == Id);
        }

        public Citizen getCitizen(string Id)
        {
            return db.Citizens.SingleOrDefault(b => b.Id == Id);
        }

        public void CreateGasStation(string Email, string Name)
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

        public void CreateInvestigator(string Id, string Name)
        {
            throw new NotImplementedException();
        }

        public void CreateCitizen(string Email, string Name)
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