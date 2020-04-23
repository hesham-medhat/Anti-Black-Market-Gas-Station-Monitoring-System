using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GSMS.Models;
using Microsoft.AspNet.Identity;

namespace GSMS.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project OverView";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "contacts.";

            return View();
        }
    }
}