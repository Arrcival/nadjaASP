using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nadja.Models;

namespace Nadja.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "More about Nadja";

            return View();
        }

        public ActionResult Commands()
        {
            ViewBag.Message = "Commands list";

            return View();
        }

        public ActionResult Journal()
        {
            ViewBag.Message = "The journal log";
            ViewBag.Journal = Models.Journal.GetLogs();

            return View();
        }
        
    }
}