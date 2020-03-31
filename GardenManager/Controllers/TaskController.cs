using GardenManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenManager.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            List<t_tache> tasksList = new List<t_tache>();
            using (db_gmEntities1 DbModel = new db_gmEntities1())
            {
                tasksList = DbModel.t_tache.ToList<t_tache>();
            }

            return View(tasksList);
        }

        [HttpGet]
        public JsonResult tasksList()
        {
            List<t_tache> taskList = new List<t_tache>();
            using (db_gmEntities1 DbModel = new db_gmEntities1())
            {
                taskList = DbModel.t_tache.ToList();
            }

            return Json(taskList, JsonRequestBehavior.AllowGet);
        }

        // GET: Task/Details/5
        public ActionResult Details(int id)
        {
            t_tache task = new t_tache();
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                task = dbModel.t_tache.Where(x => x.tacId == id).FirstOrDefault();
            }
            return View(task);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View(new t_tache());
        }

        // POST: Task/Create
        [HttpPost]
        public ActionResult Create(t_tache task)
        {
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                dbModel.t_tache.Add(task);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int id)
        {
            t_tache task = new t_tache();
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                task = dbModel.t_tache.Where(x => x.tacId == id).FirstOrDefault();
            }
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public ActionResult Edit(t_tache task)
        {
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                dbModel.Entry(task).State = System.Data.EntityState.Modified;
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            t_tache task = new t_tache();
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                task = dbModel.t_tache.Where(x => x.tacId == id).FirstOrDefault();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (db_gmEntities1 dbModel = new db_gmEntities1())
            {
                t_tache task = dbModel.t_tache.Where(x => x.tacId == id).FirstOrDefault();
                dbModel.t_tache.Remove(task);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
