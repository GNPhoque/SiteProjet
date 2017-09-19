using SiteProjet.Models;
using SiteProjet.Models.LocalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RDVMEDECINS.Controllers
{
    public class RDVMEDECINSController : Controller
    {
        DBRDVMEDECINSEntities cx = new DBRDVMEDECINSEntities();
        // GET: RDVMEDECINS
        [Authorize(Roles = "Medecin,Secretaire,Admin")]
        public ActionResult Index()
        {
            if (User.IsInRole("Medecin"))
            {
                List<MEDECINS> lMed = new List<MEDECINS>();
                lMed.Add(cx.MEDECINS.Single(x => x.EMAIL == User.Identity.Name));
                return View(lMed);
            }
            if (User.IsInRole("Secretaire")|| User.IsInRole("Admin"))
            {
                List<MEDECINS> lMed = new List<MEDECINS>();
                foreach (var item in cx.MEDECINS)
                {
                    lMed.Add(item);
                }
                return View(lMed);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Calendrier(int idMed, DateTime jour)
        {
            MEDECINS med = cx.MEDECINS.Single(x => x.ID == idMed);
            ViewData["idMed"] = idMed;
            ViewData["titreMed"] = med.TITRE;
            ViewData["nomMed"] = med.NOM;
            ViewData["prenomMed"] = med.PRENOM;
            ViewData["jour"] = jour;
            List<RV> lRV = new List<RV>();
            foreach (var item in cx.RV.Where(x => x.CRENEAUX.ID_MEDECIN == idMed && x.JOUR == jour))
            {
                lRV.Add(item);
            }
            return View(lRV);
        }

        [HttpGet]
        public ActionResult Reserver(int idMed, int hDebut, int mDebut, int hFin, int mFin, DateTime jour)
        {
            MEDECINS med = new MEDECINS();
            med = cx.MEDECINS.Single(x => x.ID == idMed);
            ViewData["idMed"] = idMed;
            ViewData["titreMed"] = med.TITRE;
            ViewData["nomMed"] = med.NOM;
            ViewData["prenomMed"] = med.PRENOM;
            ViewData["hDebut"] = hDebut;
            ViewData["mDebut"] = mDebut;
            ViewData["hFin"] = hFin;
            ViewData["mFin"] = mFin;
            ViewData["jour"] = jour;
            List<CLIENTS> lCl = new List<CLIENTS>();
            foreach (var item in cx.CLIENTS)
            {
                lCl.Add(item);
            }
            return View(lCl);
        }

        [HttpPost]
        public ActionResult Reserver(int idClient, int idMed, int hDebut, int mDebut, int hFin, int mFin, DateTime jour)
        {
            CRENEAUX cr = new CRENEAUX() { ID = (int)cx.CRENEAUX.Max(x => x.ID) + 1, HDEBUT = hDebut, MDEBUT = mDebut, HFIN = hFin, MFIN = mFin, ID_MEDECIN = idMed };
            cr.MEDECINS = cx.MEDECINS.Single(x => x.ID == idMed);
            RV rv = new RV() { ID = (int)cx.RV.Max(x => x.ID) + 1, ID_CLIENT = idClient, ID_CRENEAU = cr.ID, JOUR = jour };
            rv.CRENEAUX = cr;
            rv.CLIENTS = cx.CLIENTS.Single(x => x.ID == idClient);
            cx.CRENEAUX.Add(cr);
            cx.RV.Add(rv);
            cx.SaveChanges();
            List<RV> lRv = new List<RV>();
            foreach (var item in cx.RV.Where(x => x.CRENEAUX.ID_MEDECIN == cr.ID_MEDECIN && x.JOUR == rv.JOUR))
            {
                lRv.Add(item);
            }
            MEDECINS med = cx.MEDECINS.Single(x => x.ID == idMed);
            ViewData["idMed"] = idMed;
            ViewData["titreMed"] = med.TITRE;
            ViewData["nomMed"] = med.NOM;
            ViewData["prenomMed"] = med.PRENOM;
            ViewData["jour"] = jour;
            return View("Calendrier", lRv);
        }

        public ActionResult NouveauClient(int idMed, int hDebut, int mDebut, int hFin, int mFin, DateTime jour)
        {
            ViewData["idMed"] = idMed;
            ViewData["hDebut"] = hDebut;
            ViewData["mDebut"] = mDebut;
            ViewData["hFin"] = hFin;
            ViewData["mFin"] = mFin;
            ViewData["jour"] = jour;
            return View();
        }

        [HttpPost]
        public ActionResult NouveauClient(string titreClient, string nomClient, string prenomClient, int idMed, int hDebut, int mDebut, int hFin, int mFin, DateTime jour)
        {
            CLIENTS cl = new CLIENTS() { ID = cx.CLIENTS.Max(x => x.ID) + 1, TITRE = titreClient, NOM = nomClient, PRENOM = prenomClient };
            cx.CLIENTS.Add(cl);
            cx.SaveChanges();
            ViewData["idMed"] = idMed;
            ViewData["hDebut"] = hDebut;
            ViewData["mDebut"] = mDebut;
            ViewData["hFin"] = hFin;
            ViewData["mFin"] = mFin;
            ViewData["jour"] = jour;
            return RedirectToAction("Reserver", new { idMed, hDebut, mDebut, hFin, mFin, jour });
        }

        public ActionResult Supprimer(int idRv, int idCl, int idCr, DateTime jour)
        {
            CLIENTS cl = cx.CLIENTS.ToList().Single(x => x.ID == idCl);
            CRENEAUX cr = cx.CRENEAUX.ToList().Single(x => x.ID == idCr);
            ViewData["nomClient"] = cl.NOM;
            ViewData["prenomClient"] = cl.PRENOM;
            ViewData["jour"] = jour;
            ViewData["hFin"] = cr.HFIN;
            ViewData["mFin"] = cr.MFIN;
            ViewData["hDebut"] = cr.HDEBUT;
            ViewData["mDebut"] = cr.MDEBUT;
            ViewData["idRv"] = idRv;
            ViewData["idCr"] = idCr;
            return View();
        }

        [HttpPost]
        public ActionResult Supprimer(int idRv, int idCr)
        {
            RV rv = cx.RV.Single(x => x.ID == idRv);
            CRENEAUX cr = cx.CRENEAUX.Single(x => x.ID == idCr);
            MEDECINS med = cx.MEDECINS.Single(x => x.ID == cr.MEDECINS.ID);
            int idMed = (int)cr.ID_MEDECIN;
            DateTime jour = (DateTime)rv.JOUR;
            ViewData["idMed"] = idMed;
            ViewData["titreMed"] = med.TITRE;
            ViewData["nomMed"] = med.NOM;
            ViewData["prenomMed"] = med.PRENOM;
            ViewData["jour"] = jour;
            cx.RV.Remove(rv);
            cx.SaveChanges();
            cx.CRENEAUX.Remove(cr);
            cx.SaveChanges();
            List<RV> lRv = new List<RV>();
            foreach (var item in cx.RV.Where(x => x.CRENEAUX.ID_MEDECIN == idMed && x.JOUR == jour))
            {
                lRv.Add(item);
            }
            return View("Calendrier", lRv);
        }
    }
}