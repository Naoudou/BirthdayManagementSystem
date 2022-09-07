using BirthdayManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirthdayManagementSystem.Controllers
{
    [Authorize]
    public class Home_secController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();
        public ActionResult Index()
        {
            ViewBag.NombreTotal_sj = db.Jesuites.Count();
            ViewBag.Nombre_anniv_du_jour= db.Jesuites.Where(x=>x.DateNassance.Month.Equals(DateTime.Now.Month) && x.DateNassance.Day.Equals(DateTime.Now.Day)).Count();
            ViewBag.NombreMessage_anni = db.Messages.Count();
            ViewBag.NombreUtilisateur = db.Jesuites.Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}