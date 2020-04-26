using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSMS.Models;
using GSMS.Services;
using static GSMS.Models.GasStationViewModels;

namespace GSMS.Controllers
{
    public class GasStationController : Controller
    {
        private GasStationService Service { get; set; }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public GasStationController()
        {
            Service = new GasStationService();
        }

        [HttpGet]
        public ActionResult Refill(RefillModel model)
        {
            GasStation gs = (GasStation)Session["user"];
            bool grant = (model.RefillQuantity + gs.EstimatedFuelQuantity <= gs.TotalTankSize);
            Service.SubmitRefill(model.RefillQuantity, gs.Id, grant);
            return RedirectToLocal("/Account/Admin");
        }

        [HttpGet]
        public ActionResult serveUser(ServeUserModel model)
        {
            GasStation gs = (GasStation)Session["user"];
            Service.SubmitServeUSer(model.ServeUserQuantity, model.UserName, gs.Id);
            return RedirectToLocal("/Account/Admin");
        }
    }
}