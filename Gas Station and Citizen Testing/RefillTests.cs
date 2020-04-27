using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GSMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gas_Station_and_Citizen_Testing
{
    [TestClass]
    public class RefillTests
    {

        private System.Collections.Generic.Dictionary<string, string> _userIds = new System.Collections.Generic.Dictionary<string, string>();

        public RefillTests()
        {
            _userIds.Add("gstest1", "e4ae80bb-58b2-421a-817d-94496a24dbee");
            _userIds.Add("gstest2", "7abc44c9-e6e9-48ac-9fff-a01c14ce0b7f");
            _userIds.Add("ctest1", "491d7850-3e48-4ccb-a99f-98fc4ef9cfa8");
            _userIds.Add("ctest2", "af5a5ce2-a221-44ee-9a14-0959e6d3aa4a");
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 111
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 0
        /// Assert T0talTankSize == 111
        /// Request Refill on gasStationService.SubmitRefill for a granted amount of 111
        /// Assert estimated == 111
        /// Assert TotalTankSize == 111
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillFullFromZero()
        {
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 0;
            gstest1.TotalTankSize = 111;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 111);

            /* Procedure */
            gasStationService.SubmitRefill(111, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 111);
            Assert.IsTrue(gstest1.TotalTankSize == 111);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 111);
            Assert.IsTrue(mostRecentRefill.Granted);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 50 and TotalTankSize = 100
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 50
        /// Assert T0talTankSize == 100
        /// Request Refill on gasStationService.SubmitRefill for a granted amount of 50
        /// Assert estimated == 100
        /// Assert TotalTankSize == 100
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillHalfPlusHalf()
        {
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 50;
            gstest1.TotalTankSize = 100;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 50);
            Assert.IsTrue(gstest1.TotalTankSize == 100);

            /* Procedure */
            gasStationService.SubmitRefill(50, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 100);
            Assert.IsTrue(gstest1.TotalTankSize == 100);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 50);
            Assert.IsTrue(mostRecentRefill.Granted);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 51
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for a granted amount of 50
        /// Assert estimated == 101
        /// Assert TotalTankSize == 101
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillFullFromCeilHalf()
        {
            GSMS.Entities dbContext = new GSMS.Entities();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 51;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 51);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(50, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 101);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 50);
            Assert.IsTrue(mostRecentRefill.Granted);
        }


        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 0
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for a granted amount of 51
        /// Assert estimated == 51
        /// Assert TotalTankSize == 101
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillCeilHalf()
        {

            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 0;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(51, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 51);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 51);
            Assert.IsTrue(mostRecentRefill.Granted);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 0
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for a granted amount of 50
        /// Assert estimated == 50
        /// Assert TotalTankSize == 101
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillFloorHalf()
        {
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();

            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 0;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            gasStationService.SubmitRefill(50, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 50);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 50);
            Assert.IsTrue(mostRecentRefill.Granted);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 0
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for an ungranted amount of 102
        /// Assert estimated == 0
        /// Assert TotalTankSize == 101
        /// Assert Refill History
        /// </summary>
        [TestMethod]
        public void TestRefillFullPlus1()
        {
            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 0;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(102, gstest1.Id, false);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity == 102);
            Assert.IsTrue(!mostRecentRefill.Granted);
        }


        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 0 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 0
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for granted amount of 0
        /// Assert estimated == 0
        /// Assert TotalTankSize == 101
        /// Assert Refill History was not added
        /// </summary>
        [TestMethod]
        public void TestRefillZero()
        {
            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 0;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(0, gstest1.Id, true);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 0);
            Assert.IsTrue(gstest1.TotalTankSize == 101);


            RefillHistory mostRecentRefill = gstest1.RefillHistories.ToList()[gstest1.RefillHistories.Count - 1];

            Assert.IsTrue(mostRecentRefill.Quantity != 0);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 20 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 20
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for ungranted amount of -10
        /// Assert estimated == 20
        /// Assert TotalTankSize == 101
        /// </summary>
        [TestMethod]
        public void TestRefillNegativeFromEstimated()
        {
            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 20;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 20);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(-10, gstest1.Id, false);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 20);
            Assert.IsTrue(gstest1.TotalTankSize == 101);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 20 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 20
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for ungranted amount of -50
        /// Assert estimated == 20
        /// Assert TotalTankSize == 101
        /// </summary>
        [TestMethod]
        public void TestRefillNegativeOverEstimated()
        {
            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 20;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 20);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(-50, gstest1.Id, false);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 20);
            Assert.IsTrue(gstest1.TotalTankSize == 101);
        }

        /// <summary>
        /// Init: Create gas station: gstest1 with EstimatedFuelQuantity = 101 and TotalTankSize = 101
        /// Save changes to db
        /// Assert EstimatedFuelQuantity == 20
        /// Assert T0talTankSize == 101
        /// Request Refill on gasStationService.SubmitRefill for ungranted amount of -101
        /// Assert estimated == 101
        /// Assert TotalTankSize == 101
        /// Assert.IsTrue(mostRecentRefillHistory.Quantity == -101 && !mostRecentRefillHistory.Granted);
        /// </summary>
        [TestMethod]
        public void TestRefillNegativeTankSizeFromFull()
        {
            /* Initialization */
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            gstest1.EstimatedFuelQuantity = 101;
            gstest1.TotalTankSize = 101;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 101);
            Assert.IsTrue(gstest1.TotalTankSize == 101);

            /* Procedure */
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();
            gasStationService.SubmitRefill(-101, gstest1.Id, false);

            /* Assert procedure */
            dbContext = new GSMS.Entities();

            gstest1 = dbContext.GasStations.Find(_userIds["gstest1"]);

            Assert.IsTrue(gstest1.EstimatedFuelQuantity == 101);
            Assert.IsTrue(gstest1.TotalTankSize == 101);
        }

    }
}
