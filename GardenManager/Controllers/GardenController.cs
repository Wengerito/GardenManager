using GardenManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GardenManager.Controllers
{
    public class GardenController : Controller
    {
        // GET: Garden
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult plantsList()
        {
            List<t_plante> plantsList = new List<t_plante>();
            using (db_gmEntities DbModel = new db_gmEntities())
            {
                plantsList = DbModel.t_plante.ToList();
            }

            return Json(plantsList, JsonRequestBehavior.AllowGet);
        }

        // GET: Garden/Create
        public ActionResult Create()
        {
            return View(new t_jardin());
        }

        // POST: Garden/Create
        [HttpPost]
        public ActionResult Create(t_jardin garden)
        {
            // Variables for FK ID
            long gardenId;
            long tileId;

            // Variable protocol
            string tileProtocol;

            // Save garden
            using (db_gmEntities4 DbModel = new db_gmEntities4())
            {
                DbModel.t_jardin.Add(garden);
                DbModel.SaveChanges();
                gardenId = garden.jarId;
            }

            // Save tiles of the garden
            string[] strList = garden.jarDisposition.Split('X');
            foreach (string s in strList)
            {
                string[] tabStr = s.Split(',');
                t_tile newTile = new t_tile();
                newTile.tilDate = DateTime.Now;
                newTile.jarId = gardenId;
                newTile.tilPosition = tabStr[0];
                newTile.tilPlant = tabStr[1];
                using (db_gmEntities4 DbModel = new db_gmEntities4())
                {
                    DbModel.t_tile.Add(newTile);
                    DbModel.SaveChanges();
                    tileId = newTile.tilId;
                }

                // Save protocols of tile
                // Get the plant protocol
                using (db_gmEntities dbModel = new db_gmEntities())
                {
                    t_plante plant = dbModel.t_plante.Where(x => x.plaName == newTile.tilPlant).FirstOrDefault();
                    tileProtocol = plant.plaProtocol;
                }

                // Make protocol objects from protocols string
                int proNumber = tileProtocol.Split(')').Length - 1;
                var proTab = new String[proNumber];

                for (int i = 0; i < proTab.Length; i++)
                {
                    int start = tileProtocol.IndexOf((i + 1 + ")").ToString());
                    int end;
                    if (i == proTab.Length - 1)
                    {
                        end = tileProtocol.Length;
                    }
                    else
                    {
                        end = tileProtocol.IndexOf((i + 2 + ")").ToString());
                    }
                    string proString = tileProtocol.Substring(start+2, end - start-2);
                    proTab[i] = proString;
                }
                
                //Months array:
                String[] months = {"janvier","fevrier","mars","avril","mai","juin","juillet","août","septembre","octobre","novembre","decembre"};

                //Today month
                CultureInfo culture = new CultureInfo("fr-Fr");
                string todayMonth = culture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToLower();

                //Tasks array: 
                String[] tasks;
                using (db_gmEntities1 DbModel = new db_gmEntities1())
                {
                    var taskName = from m in DbModel.t_tache select m.tacName.ToLower();
                    tasks = taskName.ToArray();
                }

                //Time unit array:
                String[] timeUnit = { "jours", "semaines", "mois" };

                //first protocol
                t_protocole newProt = new t_protocole();
                foreach (string sMonth in months)
                {
                    if (proTab[0].Contains(sMonth))
                    {
                        newProt.proIsDone = false;
                        newProt.tilId = tileId;
                        newProt.proDateTask = DateTime.Now;
                        newProt.proToDo = false;
                    }
                }
                foreach (string sTask in tasks)
                {
                    if (proTab[0].Contains(sTask))
                    {
                        newProt.proTask = sTask;
                    }
                }
                using (db_gmEntities4 DbModel = new db_gmEntities4())
                {
                    DbModel.t_protocole.Add(newProt);
                    DbModel.SaveChanges();
                }

                //All protocols between start and end
                for (int i = 1; i < proTab.Length-1; i++)
                {
                    t_protocole newProtx = new t_protocole();
                    newProtx.tilId = tileId;
                    newProtx.proIsDone = false;
                    newProtx.proToDo = false;
                    foreach (string sTask in tasks)
                    {
                        if (proTab[i].Contains(sTask))
                        {
                            newProtx.proTask = sTask;
                        }
                    }
                    var timeNbr = 1;
                    foreach (char c in proTab[i])
                    {
                        if (Char.IsDigit(c))
                        {
                            timeNbr = int.Parse(c.ToString());
                        }
                    }
                    string tUnit="";
                    foreach (string sTimeUnit in timeUnit)
                    {
                        if (proTab[i].Contains(sTimeUnit))
                        {
                            tUnit = sTimeUnit;
                        }
                    }

                    DateTime today = DateTime.Now;
                    switch (tUnit)
                    {
                        case "":
                            break;
                        case "jours":
                            TimeSpan daySpan = new TimeSpan(timeNbr, 0, 0, 0, 0);
                            newProtx.proDateTask = today.Add(daySpan);
                            break;
                        case "semaines":
                            TimeSpan weekSpan = new TimeSpan(timeNbr * 7, 0, 0, 0, 0);
                            newProtx.proDateTask = DateTime.Now.Add(weekSpan);
                            break;
                        case "mois":
                            TimeSpan monthSpan = new TimeSpan(timeNbr*30,0,0,0,0);
                            newProtx.proDateTask = DateTime.Now.Add(monthSpan);
                            break;
                        default:
                            break;
                    }

                    using (db_gmEntities4 DbModel = new db_gmEntities4())
                    {
                        DbModel.t_protocole.Add(newProtx);
                        DbModel.SaveChanges();
                    }
                }

                //Last Protocol
                t_protocole newProtEnd = new t_protocole();
                newProtEnd.tilId = tileId;
                newProtEnd.proIsDone = false;
                newProtEnd.proToDo = false;
                newProtEnd.proTask = "récolter";
                int lpTimeNbr = 1;
                string lptUnit = "";
                foreach (string sTimeUnit in timeUnit)
                {
                    if (proTab[proTab.Length - 1].Contains(sTimeUnit))
                    {
                        lptUnit = sTimeUnit;
                    }
                }

                foreach (char c in proTab[proTab.Length-1])
                {
                    if (Char.IsDigit(c))
                    {
                        lpTimeNbr = int.Parse(c.ToString());
                    }
                }
                switch (lptUnit)
                {
                    case "":
                        break;
                    case "semaines":
                        TimeSpan weekSpan = new TimeSpan(lpTimeNbr * 7, 0, 0, 0, 0);
                        newProtEnd.proDateTask = DateTime.Now.Add(weekSpan);
                        break;
                    case "mois":
                        TimeSpan monthSpan = new TimeSpan(lpTimeNbr * 30, 0, 0, 0, 0);
                        newProtEnd.proDateTask = DateTime.Now.Add(monthSpan);
                        break;
                    default:
                        break;
                }

                using (db_gmEntities4 DbModel = new db_gmEntities4())
                {
                    DbModel.t_protocole.Add(newProtEnd);
                    DbModel.SaveChanges();
                }
            }




            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult gardensList()
        {
            List<t_jardin> gardenList = new List<t_jardin>();
            using (db_gmEntities4 DbModel = new db_gmEntities4())
            {
                DbModel.Configuration.LazyLoadingEnabled = false;
                DbModel.Configuration.ProxyCreationEnabled = false;
                gardenList = DbModel.t_jardin.ToList();
            }
            
            return Json(gardenList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult tilesList(int gardenId)
        {
            List<t_tile> tilesList = new List<t_tile>();
            using (db_gmEntities4 DbModel = new db_gmEntities4())
            {
                DbModel.Configuration.LazyLoadingEnabled = false;
                DbModel.Configuration.ProxyCreationEnabled = false;
                tilesList = DbModel.t_tile.Where(x => x.jarId == gardenId).ToList();
                foreach (t_tile tile in tilesList)
                {
                    tile.t_protocole = DbModel.t_protocole.Where(y => y.tilId == tile.tilId).ToList();

                    foreach (t_protocole prot in tile.t_protocole)
                    {
                        if (prot.proDateTask<= DateTime.Now)
                        {
                            prot.proToDo = true;
                        }
                    }
                }
            }

            return Json(tilesList, JsonRequestBehavior.AllowGet);
        }
    }
}