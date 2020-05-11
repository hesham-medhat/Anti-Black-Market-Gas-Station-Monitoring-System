using System;
using GSMS;
using GSMS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Investigations_Testing
{
    [TestClass]
    public class UnderInvestigationTests
    {
        private System.Collections.Generic.Dictionary<string, string> _userIds =
            new System.Collections.Generic.Dictionary<string, string>();

        public UnderInvestigationTests()
        {
            _userIds.Add("gstest1", "e4ae80bb-58b2-421a-817d-94496a24dbee");
            _userIds.Add("gstest2", "7abc44c9-e6e9-48ac-9fff-a01c14ce0b7f");
            _userIds.Add("ctest1", "491d7850-3e48-4ccb-a99f-98fc4ef9cfa8");
            _userIds.Add("ctest2", "af5a5ce2-a221-44ee-9a14-0959e6d3aa4a");
        }

        private void InitGSByName(string gasStationName, int estimatedFuelQuantity, int totalTankSize)
        {
            InitGSByID(_userIds[gasStationName], estimatedFuelQuantity, totalTankSize);
        }

        private void InitGSByID(string gasStationID, int estimatedFuelQuantity, int totalTankSize)
        {
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();

            /* Initialization */
            GSMS.GasStation gstest1 = dbContext.GasStations.Find(gasStationID);
            gstest1.EstimatedFuelQuantity = estimatedFuelQuantity;
            gstest1.TotalTankSize = totalTankSize;
            dbContext.SaveChanges();

            /* Assert initialization */
            gstest1 = dbContext.GasStations.Find(gasStationID);
            Assert.IsTrue(gstest1.EstimatedFuelQuantity == estimatedFuelQuantity);
            Assert.IsTrue(gstest1.TotalTankSize == totalTankSize);
        }


        /// <summary>
        /// Test helper test initializer method in testing logic initializing gas stations
        /// with different tank sizes and estimated fuel quantities
        /// </summary>
        [TestMethod]
        public void TestInitGSByName()
        {
            InitGSByName("gstest1", 50, 150);
            InitGSByName("gstest1", 150, 150);
            InitGSByName("gstest1", 0, 150);
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// 
        /// Procedure:
        /// Request refill of 151
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table contains entry for "gstest1" gas station.
        /// Assert: Investigator id of this investigation exists in Investigators
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForFullPlusOneFromZero()
        {
            /* Initialization */

            string gsName = "gstest1";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
                dbContext.SaveChanges();
            }

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));

            /* Procedure */

            gsService.SubmitRefill(151, _userIds[gsName], false);

            /* Assertion */

            dbContext = new Entities();

            UnderInvestigation investigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            Assert.IsNotNull(investigation);
            Assert.IsNotNull(dbContext.Investigators.Find(investigation.InvestigatorId));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// 
        /// Procedure:
        /// Request refill of 150
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table does not contain entry for "gstest1" gas station.
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForFullFromZero()
        {
            /* Initialization */

            string gsName = "gstest1";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
                dbContext.SaveChanges();
            }

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));

            /* Procedure */

            gsService.SubmitRefill(150, _userIds[gsName], true);

            /* Assertion */

            dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 1 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// 
        /// Procedure:
        /// Request refill of 150
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table does contains entry for "gstest1" gas station.
        /// Assert: Investigator id of this investigation exists in Investigators
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForFullFromOne()
        {
            /* Initialization */

            string gsName = "gstest1";
            InitGSByName(gsName, 1, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
                dbContext.SaveChanges();
            }

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));

            /* Procedure */

            gsService.SubmitRefill(150, _userIds[gsName], false);

            /* Assertion */

            dbContext = new Entities();

            UnderInvestigation investigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            Assert.IsNotNull(investigation);
            Assert.IsNotNull(dbContext.Investigators.Find(investigation.InvestigatorId));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// 
        /// Procedure:
        /// Request refill of 76
        /// Request refill of 74
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table does not contain entry for "gstest1" gas station.
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForEvenTwoMediansFromZero()
        {
            /* Initialization */

            string gsName = "gstest1";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
                dbContext.SaveChanges();
            }

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));

            /* Procedure */

            gsService.SubmitRefill(76, _userIds[gsName], true);
            gsService.SubmitRefill(74, _userIds[gsName], true);

            /* Assertion */

            dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest2" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest2" gas station.
        /// 
        /// Procedure:
        /// Request refill of 76
        /// Request refill of 75
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table contains entry for "gstest2" gas station.
        /// Assert: Investigator id of this investigation exists in Investigators
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForHalfPlusOnePlusHalfFromZero()
        {
            /* Initialization */

            string gsName = "gstest2";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
                dbContext.SaveChanges();
            }

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));

            /* Procedure */

            gsService.SubmitRefill(76, _userIds[gsName], true);
            gsService.SubmitRefill(75, _userIds[gsName], false);

            /* Assertion */

            dbContext = new Entities();

            UnderInvestigation investigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            Assert.IsNotNull(investigation);
            Assert.IsNotNull(dbContext.Investigators.Find(investigation.InvestigatorId));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// Initialize Citizen ctest1 with quota = 50
        /// Save changes to db
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// Assert Citizen ctest1's quota == 50
        /// 
        /// Procedure:
        /// Request refill of 76
        /// Serve user ctest1 for 1
        /// Request refill of 75
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table does not contain entry for "gstest1" gas station.
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForHalfPlusOnePlusHalfFromZeroServingOneInBetween()
        {
            /* Initialization */

            string citizenName = "ctest1";

            string gsName = "gstest1";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
            }

            Citizen ctest1 = dbContext.Citizens.Find(_userIds[citizenName]);
            ctest1.Quota = 50;
            dbContext.SaveChanges();

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
            Assert.IsTrue(50 == dbContext.Citizens.Find(_userIds[citizenName]).Quota);

            /* Procedure */

            gsService.SubmitRefill(76, _userIds[gsName], true);

            gsService.SubmitServeUSer(1, citizenName, _userIds[gsName]);

            gsService.SubmitRefill(75, _userIds[gsName], true);

            /* Assertion */

            dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest2" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// Initialize Citizen ctest1 with quota = 50
        /// Save changes to db
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest2" gas station.
        /// Assert Citizen ctest1's quota == 50
        /// 
        /// Procedure:
        /// Request refill of 76
        /// Request refill of 75
        /// Serve user ctest1 for 50
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table contains entry for "gstest2" gas station.
        /// Assert: Investigator id of this investigation exists in Investigators
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForHalfPlusOnePlusHalfFromZeroServing50After()
        {
            /* Initialization */

            string citizenName = "ctest1";

            string gsName = "gstest2";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
            }

            Citizen ctest1 = dbContext.Citizens.Find(_userIds[citizenName]);
            ctest1.Quota = 50;
            dbContext.SaveChanges();

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
            Assert.IsTrue(50 == dbContext.Citizens.Find(_userIds[citizenName]).Quota);

            /* Procedure */

            gsService.SubmitRefill(76, _userIds[gsName], true);

            gsService.SubmitRefill(75, _userIds[gsName], false);

            gsService.SubmitServeUSer(50, citizenName, _userIds[gsName]);

            /* Assertion */

            dbContext = new Entities();

            UnderInvestigation investigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            Assert.IsNotNull(investigation);
            Assert.IsNotNull(dbContext.Investigators.Find(investigation.InvestigatorId));
        }

        /// <summary>
        /// Initialization:
        /// Initialize GasStation "gstest1" with 0 estimated fuel and 150 tank size
        /// Remove entry if in UnderInvestigations table
        /// Initialize Citizen ctest1 with quota = 50
        /// Save changes to db
        /// 
        /// Initialization Assertion:
        /// Assert no entry in UnderInvestigation for "gstest1" gas station.
        /// Assert Citizen ctest1's quota == 150
        /// 
        /// Procedure:
        /// Serve user ctest1 for 150
        /// Request refill of 150
        /// 
        /// Assertion:
        /// Assert: UnderInvestigations table does not contain entry for "gstest1" gas station.
        /// </summary>
        [TestMethod]
        public void TestUnderInvestigationForFullServeFullRefill()
        {
            /* Initialization */

            string citizenName = "ctest1";

            string gsName = "gstest1";
            InitGSByName(gsName, 0, 150);

            GasStationService gsService = new GasStationService();

            Entities dbContext = new Entities();
            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(_userIds[gsName]);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
            }

            Citizen ctest1 = dbContext.Citizens.Find(_userIds[citizenName]);
            ctest1.Quota = 150;
            dbContext.SaveChanges();

            /* Initialization Assertion */

            dbContext = new Entities();
            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
            Assert.IsTrue(150 == dbContext.Citizens.Find(_userIds[citizenName]).Quota);

            /* Procedure */

            gsService.SubmitServeUSer(150, citizenName, _userIds[gsName]);

            gsService.SubmitRefill(150, _userIds[gsName], true);

            /* Assertion */

            dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(_userIds[gsName]));
        }
    }
}
