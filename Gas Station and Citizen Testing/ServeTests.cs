using System;
using System.Linq;
using GSMS;
using GSMS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gas_Station_and_Citizen_Testing
{
    [TestClass]
    public class ServeTests
    {
        private System.Collections.Generic.Dictionary<string, string> _userIds = new System.Collections.Generic.Dictionary<string, string>();

        public ServeTests()
        {
            _userIds.Add("gstest1", "e4ae80bb-58b2-421a-817d-94496a24dbee");
            _userIds.Add("gstest2", "7abc44c9-e6e9-48ac-9fff-a01c14ce0b7f");
            _userIds.Add("ctest1", "491d7850-3e48-4ccb-a99f-98fc4ef9cfa8");
            _userIds.Add("ctest2", "af5a5ce2-a221-44ee-9a14-0959e6d3aa4a");
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 100 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for 15
        /// 
        /// Assert deducted serve quantity from ctest1.Quota
        /// Assert deducted serve quantity from gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history for GasStationId, CitizenId, and Quantity
        /// </summary>
        [TestMethod]
        public void TestServeCitizenHalfQuota()
        {
            int originalFuelQuantity = 100;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = 15;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity - serveQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota - serveQuantity);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory.Quantity == serveQuantity);
            Assert.IsTrue(mostRecentHistory.GasStationId == gstest1.Id);
            Assert.IsTrue(mostRecentHistory.CitizenId == ctest1.Id);
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 100 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for 30
        /// 
        /// Assert deducted serve quantity from ctest1.Quota
        /// Assert deducted serve quantity from gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history for GasStationId, CitizenId, and Quantity
        /// </summary>
        [TestMethod]
        public void TestServeCitizenFullQuota()
        {
            int originalFuelQuantity = 100;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = 30;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity - serveQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota - serveQuantity);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory.Quantity == serveQuantity);
            Assert.IsTrue(mostRecentHistory.GasStationId == gstest1.Id);
            Assert.IsTrue(mostRecentHistory.CitizenId == ctest1.Id);
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 100 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for 0
        /// 
        /// Assert same ctest1.Quota
        /// Assert same gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history was not recorded for that Quantity
        /// </summary>
        [TestMethod]
        public void TestServeCitizenZero()
        {
            int originalFuelQuantity = 100;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = 0;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory == null || mostRecentHistory.Quantity != serveQuantity);
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 100 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for -15
        /// 
        /// Assert same ctest1.Quota
        /// Assert same gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history was not recorded for that Quantity
        /// </summary>
        [TestMethod]
        public void TestServeCitizenNegative()
        {
            int originalFuelQuantity = 100;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = -15;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory == null || mostRecentHistory.Quantity != serveQuantity);
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 100 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for 31
        /// 
        /// Assert same ctest1.Quota
        /// Assert same gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history was not recorded for that Quantity
        /// </summary>
        [TestMethod]
        public void TestServeCitizenOverQuota()
        {
            int originalFuelQuantity = 100;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = 31;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory == null || mostRecentHistory.Quantity != serveQuantity);
        }

        /// <summary>
        /// Init:
        /// Initialize GasStation gstest1 with EstimatedFuelQuantity = 10 and TotalTankSize = 2000
        /// Initialize Citizen ctest1 with quota = 30
        /// Save to database
        /// 
        /// Assert all three values of initialization
        /// 
        /// Procedure:
        /// Serve ctest1 by gstest1 for 15
        /// 
        /// Assert same ctest1.Quota
        /// Assert same gstest1.EstimatedFuelQuantity
        /// 
        /// Assert transaction history was not recorded for that Quantity
        /// </summary>
        [TestMethod]
        public void TestServeUnservableWithinQuota()
        {
            int originalFuelQuantity = 10;
            int totalTankSize = 2000;
            int originalQuota = 30;
            int serveQuantity = -15;

            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = originalFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;

            GSMS.Citizen ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);
            ctest1.Name = "cstest1";
            ctest1.Quota = originalQuota;

            dbContext.SaveChanges();

            /* Assert initialization */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
            Assert.IsTrue(ctest1.Quota == originalQuota);


            /* Procedure */
            GasStationService gasStationService = new GasStationService();
            gasStationService.SubmitServeUSer(serveQuantity, ctest1.Name, gstest1.Id);

            /* Assert procedure */
            dbContext = new GSMS.Entities();
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            ctest1 = dbContext.Citizens.Find(_userIds["ctest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == originalFuelQuantity);
            Assert.IsTrue(ctest1.Quota == originalQuota);

            TransactionHistory mostRecentHistory = ctest1.TransactionHistories.ToList()[ctest1.TransactionHistories.Count - 1];
            Assert.IsTrue(mostRecentHistory == null || mostRecentHistory.Quantity != serveQuantity);
        }

    }
}
