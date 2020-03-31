using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GardenManager.Models;
using System.Data;

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

        // GET: Plant/Details/5
        public ActionResult Details(int id)
        {
            t_plante plant= new t_plante();
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
            }
            return View(plant);
        }

        // GET: Plant/Create
        public ActionResult Create()
        {
            return View(new t_plante());
        }

        // POST: Plant/Create
        [HttpPost]
        public ActionResult Create(t_plante plant)
        {
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                dbModel.t_plante.Add(plant);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Plant/Edit/5
        public ActionResult Edit(int id)
        {
            t_plante plant = new t_plante();
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
            }
            return View(plant);
        }

        // POST: Plant/Edit/5
        [HttpPost]
        public ActionResult Edit(t_plante plant)
        {
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                dbModel.Entry(plant).State = System.Data.EntityState.Modified;
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Plant/Delete/5
        public ActionResult Delete(int id)
        {
            t_plante plant = new t_plante();
            using (db_gmEntities dbModel = new db_gmEntities())
            {
                plant = dbModel.t_plante.Where(x => x.plaId == id).FirstOrDefault();
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
