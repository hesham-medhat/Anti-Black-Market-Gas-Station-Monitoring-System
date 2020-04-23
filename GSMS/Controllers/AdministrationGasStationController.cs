using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSMS.Controllers
{
    public class AdministrationGasStationController : Controller
    {
        public ActionResult History()
        {
            return View();
        }

        public static int quota()
        {
            return 120;
        }

        public ActionResult RequestRefill()
        {
            return View();
        }

        public ActionResult ServeUser()
        {
            return View();
        }
    }
}