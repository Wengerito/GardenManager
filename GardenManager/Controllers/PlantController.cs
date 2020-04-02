using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenManager.Models;
using System.Data;
using System.Data.Entity.Validation;

namespace GardenManager.Controllers
{
    public class PlantController : Controller
    {
        // GET: Plant
        public ActionResult Index()
        {
            List<t_plante> plantsList = new List<t_plante>();
            using (db_gmEntities DbModel = new db_gmEntities())
            {
                plantsList = DbModel.t_plante.ToList<t_plante>();
            }

            return View(plantsList);
        }

        //// GET: Plant/Details/5
        //public ActionResult Details(int id)
        //{
        //    t_plante plant= new t_plante();
        //    using (db_gmEntities dbModel = new db_gmEntities())
        //    {
        //        plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
        //    }
        //    return View(plant);
        //}

        // GET: Plant/Create
        public ActionResult Create()
        {
            return View(new t_plante());
        }

        // POST: Plant/Create
        [HttpPost]
        public ActionResult Create(t_plante plant)
        {
            if (string.IsNullOrEmpty(plant.plaName))
            {
                ModelState.AddModelError("plaName", "Un nom de plante est requis");
            }
            if (string.IsNullOrEmpty(plant.plaType))
            {
                ModelState.AddModelError("plaType", "Un type de plante est requis");
            }
            if (plant.plaType == plant.plaName)
            {
                ModelState.AddModelError("plaType", "Le type de la plante ne peut pas être son nom");
            }

            using (db_gmEntities dbModel = new db_gmEntities())
            {
                if (dbModel.t_plante.Where(x => x.plaId != plant.plaId).Any(x => x.plaName == plant.plaName))
                {
                    ModelState.AddModelError("plaName", "Cette plante existe déjà");
                }
            }
            if (ModelState.IsValid)
            {
                using (db_gmEntities dbModel = new db_gmEntities())
                {
                    dbModel.t_plante.Add(plant);
                    dbModel.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        // GET: Plant/Edit/5
        public ActionResult Edit(int id)
        {
            t_plante plant = new t_plante();
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                if (dbModel.t_plante.Any(x => x.plaId == id))
                {
                    plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
                }
                else
                {
                    return RedirectToAction("Index");
                }
                
            }
            return View(plant);
        }

        // POST: Plant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(t_plante plant)
        {
            if (string.IsNullOrEmpty(plant.plaName))
            {
                ModelState.AddModelError("plaName", "Un nom de plante est requis");
            }
            if (string.IsNullOrEmpty(plant.plaType))
            {
                ModelState.AddModelError("plaType", "Un type de plante est requis");
            }
            if (plant.plaType==plant.plaName)
            {
                ModelState.AddModelError("plaType", "Le type de la plante ne peut pas être son nom");
            }

            using (db_gmEntities dbModel = new db_gmEntities())
            {
                if (dbModel.t_plante.Where(x => x.plaId != plant.plaId).Any(x=>x.plaName==plant.plaName))
                {
                    ModelState.AddModelError("plaName", "Cette plante existe déjà");
                }
            }

            if (ModelState.IsValid)
            {
                using (db_gmEntities dbModel = new db_gmEntities())
                {
                    dbModel.Entry(plant).State = System.Data.EntityState.Modified;
                    dbModel.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        // GET: Plant/Delete/5
        public ActionResult Delete(int id)
        {
            t_plante plant = new t_plante();
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                if (dbModel.t_plante.Any(x => x.plaId == id))
                {
                    plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            return View(plant);
        }

        // POST: Plant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                t_plante plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
                dbModel.t_plante.Remove(plant);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
