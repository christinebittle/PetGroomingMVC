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

namespace PetGrooming.Views
{
    public class GroomBookingController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();


        // GET: GroomBooking
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(string bookingsearchkey)
        {
           


            //empty variable declaration
            List<GroomBooking> Bookings;

            if (bookingsearchkey!="" && bookingsearchkey!=null)
            {
                //SQL equivalent
                

                Bookings =
                    db.GroomBookings
                    .Where(booking =>
                        booking.Groomer.GroomerFName.Contains(bookingsearchkey) ||
                        booking.Groomer.GroomerLName.Contains(bookingsearchkey) ||
                        booking.Owner.OwnerFname.Contains(bookingsearchkey) ||
                        booking.Owner.OwnerLname.Contains(bookingsearchkey) ||
                        booking.Pet.PetName.Contains(bookingsearchkey)
                     )
                    .ToList();
            }
            else
            {
                Bookings = db.GroomBookings.ToList();
            }
            

            return View(Bookings);
        }

    }
}