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
using System.Globalization; //for cultureinfo.invariantculture

namespace PetGrooming.Controllers
{
    public class GroomerController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Groomer
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            //TODO: Search Functionality
            List<Groomer> Groomers = db.Groomers.ToList();
            return View(Groomers);
        }

        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Add(string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        {
            Groomer NewGroomer = new Groomer();
            NewGroomer.GroomerFName = GroomerFName;
            NewGroomer.GroomerLName = GroomerLName;
            NewGroomer.GroomerDOB = DateTime.ParseExact(GroomerDOB,"yyyy-MM-dd",CultureInfo.InvariantCulture);
            NewGroomer.GroomerRate = (int)(GroomerRate*100);

            db.Groomers.Add(NewGroomer);
            db.SaveChanges();



            return RedirectToAction("List");
        }


        public ActionResult Update(int id)
        {
            
            Groomer SelectedGroomer = db.Groomers.Find(id);

            return View(SelectedGroomer);

        }

        [HttpPost]
        public ActionResult Update(int id, string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        {
            Groomer SelectedGroomer = db.Groomers.Find(id);
            SelectedGroomer.GroomerFName = GroomerFName;
            SelectedGroomer.GroomerLName = GroomerLName;
            SelectedGroomer.GroomerDOB = DateTime.ParseExact(GroomerDOB, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            SelectedGroomer.GroomerRate = (int)(GroomerRate * 100);

            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {
            Groomer SelectedGroomer = db.Groomers.Find(id);
            List<GroomBooking> Bookings = db.GroomBookings.Where(booking => booking.GroomerID == id).ToList();

            ShowGroomer ViewModel = new ShowGroomer();
            ViewModel.groomer = SelectedGroomer;
            ViewModel.bookings = Bookings;


            return View(ViewModel);
        }

        public ActionResult ConfirmDelete(int id)
        {
            Groomer SelectedGroomer = db.Groomers.Find(id);

            return View(SelectedGroomer);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Groomer SelectedGroomer = db.Groomers.Find(id);
            db.Groomers.Remove(SelectedGroomer);
            db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}