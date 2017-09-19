using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteProjet.Controllers
{
    public class IMCController : Controller
    {
        // GET: IMC
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(float taille, float poids)
        {
            ServiceReference1.Service1Client svc = new ServiceReference1.Service1Client();
            string s = svc.GetIMC(taille, poids);
            return RedirectToAction("Resultat", new { taille, poids });
          
        }


        public ActionResult Resultat(float taille, float poids)
        {
            ServiceReference1.Service1Client svc = new ServiceReference1.Service1Client();
            string s = svc.GetIMC( taille, poids);
            ViewData["res"] = s;
            return View("Resultat");
        }
    }
}