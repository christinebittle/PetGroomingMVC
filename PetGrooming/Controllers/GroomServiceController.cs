using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using PetGrooming.Models.ViewModels;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class GroomServiceController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();

        // GET: GroomService/List
        public ActionResult List(string servicesearchkey, int pagenum = 0)
        {
            //How could we modify this to include a search bar?
            List<GroomService> groomservices = db
                .GroomServices
                .Where(g => (servicesearchkey != null) ? g.ServiceName.Contains(servicesearchkey) : true)
                .ToList();

            //start of pagination algorithm (LINQ techniques)
            int perpage = 3;
            int petcount = groomservices.Count();
            int maxpage = (int)Math.Ceiling((decimal)petcount / perpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(perpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                groomservices = db.GroomServices
                    .Where(g => (servicesearchkey != null) ? g.ServiceName.Contains(servicesearchkey) : true)
                    .OrderBy(g=>g.GroomServiceID)
                    .Skip(start)
                    .Take(perpage)
                    .ToList();
            }
            //end of pagination algorithm (LINQ techniques)

            return View(groomservices);

        }

        public ActionResult New()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Add(string ServiceName, decimal ServiceCost, int ServiceDuration)
        {
            //cost is represented in the system as an integer
            int Cost = (int)(ServiceCost * 100);

            string query = "insert into GroomServices (ServiceName, ServiceCost, ServiceDuration) values (@ServiceName, @Cost, @ServiceDuration)";

            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@ServiceName", ServiceName);
            sqlparams[1] = new SqlParameter("@Cost", Cost);
            sqlparams[2] = new SqlParameter("@ServiceDuration", ServiceDuration);
            

            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {
            string query = "select * from GroomServices where GroomServiceID = @id";
            var parameter = new SqlParameter("@id", id);
            GroomService groomservice = db.GroomServices.SqlQuery(query, parameter).FirstOrDefault();

            return View(groomservice);
        }

        //
        public ActionResult Update(int id)
        {
            string query = "select * from GroomServices where GroomServiceID = @id";
            var parameter = new SqlParameter("@id", id);
            GroomService groomservice = db.GroomServices.SqlQuery(query, parameter).FirstOrDefault();

            return View(groomservice);
        }
        [HttpPost]
        public ActionResult Update(int id, string ServiceName, decimal ServiceCost, int ServiceDuration)
        {
            

            string query = "update GroomServices set ServiceName = @ServiceName, ServiceCost = @ServiceCost, ServiceDuration=@ServiceDuration where GroomServiceID = @id";
            
            //cost is represented in the system as an integer
            int Cost = (int)(ServiceCost * 100);

            Debug.WriteLine("Cost is"+Cost);

            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@ServiceName", ServiceName);
            sqlparams[2] = new SqlParameter("@ServiceCost", Cost);
            sqlparams[3] = new SqlParameter("@ServiceDuration", ServiceDuration);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }


        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from GroomServices where GroomServiceID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            GroomService groomservice = db.GroomServices.SqlQuery(query, param).FirstOrDefault();
            return View(groomservice);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "delete from GroomServices where GroomServiceID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);


          
            return RedirectToAction("List");
        }



    }
}