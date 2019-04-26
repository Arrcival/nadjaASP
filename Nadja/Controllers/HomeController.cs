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

        public ActionResult Changelog()
        {
            ViewBag.Message = "Changelog";
            return View();
        }

        public ActionResult Slangs()
        {
            ViewBag.Message = "Slangs list";
            Dal.DoConnection();
            ViewBag.Items = Dal.GetEveryItemWithSlang();
            Dal.CloseConnection();
            return View();
        }

        public ActionResult Ranks()
        {
            ViewBag.Message = "Ranks";
            Dal.DoConnection();
            List<ServerUser> listRanks = Dal.GetEveryUser();

            listRanks = Helper.GetRanking(listRanks, 25);
            Dal.CloseConnection();
            ViewBag.Users = listRanks;

            return View();
        }

        public ActionResult LuckRanks()
        {
            ViewBag.Message = "Luck Ranks";
            Dal.DoConnection();
            List<ServerUser> everyUsers = Dal.GetEveryUser();
            
            int max = 25;
            if (everyUsers.Count < max)
                max = everyUsers.Count;

            List<ServerUser> sorted = Helper.GetLuckRanking(everyUsers, max);
                        

            Dal.CloseConnection();
            
            ViewBag.Users = sorted;

            return View();
        }

    }
}