namespace GSMS.Services
{
    public class InvestigatorService
    {
        private Entities db = new Entities();
        public void respond(string GasstaionId, string InvestigatorId, int Severity)
        {
            FlaggedStation flaggedStation = new FlaggedStation()
            {
                Severity = Severity,
                InvestigatorId = InvestigatorId,
                GasStationId = GasstaionId
            };
            FlaggedStation newFlaggedStation = db.FlaggedStations.Find(GasstaionId);
            if (newFlaggedStation == null)
            {
                db.FlaggedStations.Add(flaggedStation);
            } else
            {
                newFlaggedStation.Severity = Severity;
                newFlaggedStation.InvestigatorId = InvestigatorId;
            }
            UnderInvestigation underInvestigation = new UnderInvestigation()
            {
                GasStationId = GasstaionId,
                InvestigatorId = InvestigatorId
            };
            db.UnderInvestigations.Attach(underInvestigation);
            db.UnderInvestigations.Remove(underInvestigation);
            db.SaveChanges();
        }
    }
}