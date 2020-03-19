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


        public ActionResult List(string groomersearchkey)
        {
            

            Debug.WriteLine("The search key is "+groomersearchkey);

            if (groomersearchkey!=null && groomersearchkey!="")
            {
                //SQL : 
                // select * from groomers 
                // where groomerfname like '%@key%' OR
                // groomerlname like '%key%'
                List<Groomer> Groomers = db.Groomers
                    .Where(groomer => 
                        groomer.GroomerFName.Contains(groomersearchkey) ||
                        groomer.GroomerLName.Contains(groomersearchkey)
                    )
                    .ToList();
                return View(Groomers);
            }
            else
            {
                List<Groomer> Groomers = db.Groomers.ToList();
                return View(Groomers);
            }
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

            //SQL equivalent : INSERT INTO GROOMERS (GroomerFname, .. ) VALUES (@GroomerFname..)
            db.Groomers.Add(NewGroomer);
           
            db.SaveChanges();



            return RedirectToAction("List");
        }


        public ActionResult Update(int id)
        {
            //select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);

            return View(SelectedGroomer);

        }

        [HttpPost]
        public ActionResult Update(int id, string GroomerFName, string GroomerLName, string GroomerDOB, decimal GroomerRate)
        {
         
            //SQL Equivalent : select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);
            SelectedGroomer.GroomerFName = GroomerFName;
            SelectedGroomer.GroomerLName = GroomerLName;
            SelectedGroomer.GroomerDOB = DateTime.ParseExact(GroomerDOB, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            SelectedGroomer.GroomerRate = (int)(GroomerRate * 100);

            //SQL equivalent: update groomers set groomerFname = @groomerfname, groomerLname = @groomerlname .. 
            // where groomerid = @id
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Show(int id)
        {
            //sql equivalent : select * from groomers where groomerid = @id
            Groomer SelectedGroomer = db.Groomers.Find(id);
            //need a list of bookings associated with that groomer
            //sql equivalent : 
            //select * from groombookings where groombookings.groomerid = @id
            //[db.GroomBookings] => Database context groombookings table
            //[.Where()] => maps to the where clause in SQL
            //[booking => booking.GroomerID == id] => where groomerid = @id
            //[.ToList()] => select * from groombookings
            List<GroomBooking> Bookings = db.GroomBookings
                .Where(booking => booking.GroomerID == id)
                .ToList();

            

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
            //find the groomer
            Groomer SelectedGroomer = db.Groomers.Find(id);
            //sql equivalent : delete from groomers where groomerid=@id 
            db.Groomers.Remove(SelectedGroomer);
            
            db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}