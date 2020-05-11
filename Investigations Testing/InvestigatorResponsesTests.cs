using GSMS;
using GSMS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investigations_Testing
{
    [TestClass]
    public class InvestigatorResponsesTests
    {
        private System.Collections.Generic.Dictionary<string, string> _userIds =
            new System.Collections.Generic.Dictionary<string, string>();

        public InvestigatorResponsesTests()
        {
            _userIds.Add("gstest1", "e4ae80bb-58b2-421a-817d-94496a24dbee");
            _userIds.Add("gstest2", "7abc44c9-e6e9-48ac-9fff-a01c14ce0b7f");
            _userIds.Add("H Investigator Tester 1", "ff07e464-c3e9-40fc-b153-d9c235ba7297");
        }

        /// <summary>
        /// Initializes an investigation for given gas station by given investigator id
        /// </summary>
        private void InitInvestigation(string gasStationID, string investigatorId)
        {
            GSMS.Entities dbContext = new GSMS.Entities();
            GSMS.Services.GasStationService gasStationService = new GSMS.Services.GasStationService();

            /* Initialization */

            UnderInvestigation gasStationUnderInvestigation = dbContext.UnderInvestigations.Find(gasStationID);
            if (gasStationUnderInvestigation != null)
            {
                dbContext.UnderInvestigations.Remove(gasStationUnderInvestigation);
            }

            GSMS.UnderInvestigation underInvestigation = new GSMS.UnderInvestigation
            {
                GasStationId = gasStationID,
                InvestigatorId = investigatorId,
                IssueDatetime = DateTime.Now
            };
            dbContext.UnderInvestigations.Add(underInvestigation);

            dbContext.SaveChanges();

            /* Assert initialization */

            dbContext = new GSMS.Entities();

            UnderInvestigation investigation = dbContext.UnderInvestigations.Find(gasStationID);
            
            Assert.IsNotNull(investigation);
            Assert.IsTrue(investigation.InvestigatorId == investigatorId);
        }


        /// <summary>
        /// Test helper test initalizer method in testing logic initializing an investigation
        /// </summary>
        [TestMethod]
        public void TestInitInvestigation()
        {
            InitInvestigation(_userIds["gstest1"], _userIds["H Investigator Tester 1"]);
            InitInvestigation(_userIds["gstest2"], _userIds["H Investigator Tester 1"]);
            InitInvestigation(_userIds["gstest1"], _userIds["H Investigator Tester 1"]);
        }

        /// <summary>
        /// Initialization:
        /// Initialize investigation with investigator as H Investigator Tester 1 and GS as gstest1
        /// 
        /// Procedure:
        /// Flag 0
        /// 
        /// Assertion:
        /// Assert UnderInvestigation does not contain gstest1 GS
        /// Assert FlaggedStations contains gstest1 GS with flag severity of 0
        /// </summary>
        [TestMethod]
        public void TestFlagging0()
        {
            /* Initialization */
            
            string investigatorId = _userIds["H Investigator Tester 1"];
            string gasStationId = _userIds["gstest1"];

            int flagSeverity = 0;

            InvestigatorService investigatorService = new InvestigatorService();

            InitInvestigation(gasStationId, investigatorId);

            /* Procedure */

            investigatorService.respond(gasStationId, investigatorId, flagSeverity);

            /* Assertion */

            Entities dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(gasStationId));
            
            FlaggedStation flaggedStation = dbContext.FlaggedStations.Find(gasStationId);
            Assert.IsNotNull(flaggedStation);
            Assert.IsTrue(flaggedStation.Severity == flagSeverity);
        }

        /// <summary>
        /// Initialization:
        /// Initialize investigation with investigator as H Investigator Tester 1 and GS as gstest1
        /// 
        /// Procedure:
        /// Flag 1
        /// 
        /// Assertion:
        /// Assert UnderInvestigation does not contain gstest1 GS
        /// Assert FlaggedStations contains gstest1 GS with flag severity of 1
        /// </summary>
        [TestMethod]
        public void TestFlagging1()
        {
            /* Initialization */

            string investigatorId = _userIds["H Investigator Tester 1"];
            string gasStationId = _userIds["gstest1"];

            int flagSeverity = 1;

            InvestigatorService investigatorService = new InvestigatorService();

            InitInvestigation(gasStationId, investigatorId);

            /* Procedure */

            investigatorService.respond(gasStationId, investigatorId, flagSeverity);

            /* Assertion */

            Entities dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(gasStationId));

            FlaggedStation flaggedStation = dbContext.FlaggedStations.Find(gasStationId);
            Assert.IsNotNull(flaggedStation);
            Assert.IsTrue(flaggedStation.Severity == flagSeverity);
        }

        /// <summary>
        /// Initialization:
        /// Initialize investigation with investigator as H Investigator Tester 1 and GS as gstest1
        /// 
        /// Procedure:
        /// Flag 2
        /// 
        /// Assertion:
        /// Assert UnderInvestigation does not contain gstest1 GS
        /// Assert FlaggedStations contains gstest1 GS with flag severity of 2
        /// </summary>
        [TestMethod]
        public void TestFlagging2()
        {
            /* Initialization */

            string investigatorId = _userIds["H Investigator Tester 1"];
            string gasStationId = _userIds["gstest1"];

            int flagSeverity = 2;

            InvestigatorService investigatorService = new InvestigatorService();

            InitInvestigation(gasStationId, investigatorId);

            /* Procedure */

            investigatorService.respond(gasStationId, investigatorId, flagSeverity);

            /* Assertion */

            Entities dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(gasStationId));

            FlaggedStation flaggedStation = dbContext.FlaggedStations.Find(gasStationId);
            Assert.IsNotNull(flaggedStation);
            Assert.IsTrue(flaggedStation.Severity == flagSeverity);
        }

        /// <summary>
        /// Initialization:
        /// Initialize investigation with investigator as H Investigator Tester 1 and GS as gstest1
        /// 
        /// Procedure:
        /// Flag 0
        /// Redo initialization
        /// Flag 1
        /// Redo initialization
        /// Flag 2
        /// 
        /// Assertion:
        /// Assert UnderInvestigation does not contain gstest1 GS
        /// Assert FlaggedStations contains gstest1 GS with flag severity of 2
        /// </summary>
        [TestMethod]
        public void TestFlagging0Then1Then2()
        {
            /* Initialization */

            string investigatorId = _userIds["H Investigator Tester 1"];
            string gasStationId = _userIds["gstest1"];

            InvestigatorService investigatorService = new InvestigatorService();

            InitInvestigation(gasStationId, investigatorId);

            /* Procedure */

            investigatorService.respond(gasStationId, investigatorId, 0);
            
            InitInvestigation(gasStationId, investigatorId);

            investigatorService.respond(gasStationId, investigatorId, 1);


            InitInvestigation(gasStationId, investigatorId);

            investigatorService.respond(gasStationId, investigatorId, 2);


            /* Assertion */

            Entities dbContext = new Entities();

            Assert.IsNull(dbContext.UnderInvestigations.Find(gasStationId));

            FlaggedStation flaggedStation = dbContext.FlaggedStations.Find(gasStationId);
            Assert.IsNotNull(flaggedStation);
            Assert.IsTrue(flaggedStation.Severity == 2);
        }
    }
}
