using System.Web.Mvc;
using GSMS.Services;
using static GSMS.Models.InvestigatorViewModel;

namespace GSMS.Controllers
{
    public class InvestigatorController : Controller
    {

        private InvestigatorService Service;


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public InvestigatorController()
        {
            Service = new InvestigatorService();
        }

        [HttpGet]
        public ActionResult respond(respondModel model)
        {
            Investigator investigator = (Investigator)Session["user"];
            Service.respond(model.GasStationId, investigator.Id, model.Severity);
            return RedirectToLocal("/Account/Admin");
        }
    }
}