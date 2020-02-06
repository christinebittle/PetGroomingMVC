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
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List()
        {
            //what data do we need?
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();

            return View(myspecies);
        }

        public ActionResult Add()
        {
            //I don't need any information to do add of species.
            return View();
        }
        [HttpPost]
        public ActionResult Add(string SpeciesName)
        {
            string query = "insert into species (Name) values (@SpeciesName)";
            var parameter = new SqlParameter("@SpeciesName",SpeciesName);

            db.Database.ExecuteSqlCommand(query, parameter);
            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {
            string query = "select * from species where speciesid = @id";
            var parameter = new SqlParameter("@id", id);
            Species selectedspecies = db.Species.SqlQuery(query, parameter).FirstOrDefault();

            return View(selectedspecies);
        }

        public ActionResult Update(int id)
        {
            string query = "select * from species where speciesid = @id";
            var parameter = new SqlParameter("@id", id);
            Species selectedspecies = db.Species.SqlQuery(query, parameter).FirstOrDefault();

            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Update(int id, string SpeciesName)
        {
            string query = "update species set name = @SpeciesName where speciesid = @id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName",SpeciesName);
            sqlparams[1] = new SqlParameter("@id",id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from species where SpeciesID=@id";
            SqlParameter param = new SqlParameter("@id",id);
            Species selectedspecies = db.Species.SqlQuery(query, param).FirstOrDefault();
            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "delete from species where speciesid=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);


            //for the sake of referential integrity, unset the species for all pets
            string refquery = "update pets set SpeciesID = '' where SpeciesID=@id";
            db.Database.ExecuteSqlCommand(refquery, param); //same param as before

            return RedirectToAction("List");
        }



        // Show
        // Add
        // [HttpPost] Add
        // Update
        // [HttpPost] Update
        // (optional) delete
        // [HttpPost] Delete
    }
}