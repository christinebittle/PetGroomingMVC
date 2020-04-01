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
//needed for other sign in feature classes
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Globalization; //for cultureinfo.invariantculture

namespace PetGrooming.Views
{
    public class GroomBookingController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private PetGroomingContext db = new PetGroomingContext();
        public GroomBookingController() { }

        // GET: GroomBooking
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(string bookingsearchkey)
        {



            //empty variable declaration
            List<GroomBooking> Bookings;

            if (bookingsearchkey != "" && bookingsearchkey != null)
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

        public ActionResult Add()
        {
            AddBooking viewmodel = new AddBooking();
            viewmodel.Groomers = db.Groomers.ToList();
            viewmodel.Pets = db.Pets.ToList();
            viewmodel.Services = db.GroomServices.ToList();
            viewmodel.Owners = db.Owners.ToList();

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Add(string BookingDate, string BookingTime, decimal BookingPrice, int PetID, string OwnerID, string GroomerID, int[] BookingServices)
        {
            GroomBooking newbooking = new GroomBooking();
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            Debug.WriteLine(BookingDate + " " + BookingTime);
            newbooking.GroomBookingDate = DateTime.ParseExact(BookingDate + " " + BookingTime, "yyyy-MM-dd Hmm", CultureInfo.InvariantCulture);
            newbooking.GroomBookingPrice = (int)(BookingPrice * 100);
            newbooking.PetID = PetID;
            newbooking.OwnerID = OwnerID;
            newbooking.GroomerID = GroomerID;

            //first add booking
            db.GroomBookings.Add(newbooking);
            db.SaveChanges();

            //Microsoft REALLY doesn't like it if you try to enumerate on a null list
            if (BookingServices != null)
            {
                if (BookingServices.Length > 0)
                {
                    newbooking.GroomServices = new List<GroomService>();
                    //then add services to that booking
                    foreach (int ServiceID in BookingServices.ToList())
                    {
                        //Debug.WriteLine("ServiceID is "+ServiceID);
                        GroomService service = db.GroomServices.Find(ServiceID);
                        //Debug.WriteLine("Service Name is "+service.ServiceName);
                        newbooking.GroomServices.Add(service);
                    }
                    db.SaveChanges();
                }
            }

            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            //This is answer to question 3 on the midterm exam!!
            UpdateBooking viewmodel = new UpdateBooking();

            //get the booking
            //Include(booking=>booking.GroomServices) => join on the booking, bookingsxservices, services bridging table
            viewmodel.Booking =
                db.GroomBookings
                .Include(booking => booking.GroomServices)
                .FirstOrDefault(booking => booking.GroomBookingID == id);


            //get all the pets
            viewmodel.Pets = db.Pets.ToList();

            //get all the owners
            viewmodel.Owners = db.Owners.ToList();

            //get all the groomers
            viewmodel.Groomers = db.Groomers.ToList();

            //get all the services
            viewmodel.Services = db.GroomServices.ToList();


            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(int id, string BookingDate, string BookingTime, decimal BookingPrice, int PetID, string OwnerID, string GroomerID, int[] BookingServices)
        {
            GroomBooking booking = db
                .GroomBookings
                .Include(b => b.GroomServices)
                .FirstOrDefault(b => b.GroomBookingID == id);

            //remove any existing services attached to this booking
            foreach (var service in booking.GroomServices.ToList())
            {
                booking.GroomServices.Remove(service);
            }
            db.SaveChanges();

            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            Debug.WriteLine(BookingDate + " " + BookingTime);
            booking.GroomBookingDate = DateTime.ParseExact(BookingDate + " " + BookingTime, "yyyy-MM-dd Hmm", CultureInfo.InvariantCulture);
            booking.GroomBookingPrice = (int)(BookingPrice * 100);
            booking.PetID = PetID;
            booking.OwnerID = OwnerID;
            booking.GroomerID = GroomerID;



            //update the booking
            db.SaveChanges();

            //Microsoft REALLY doesn't like it if you try to enumerate on a null list
            if (BookingServices != null)
            {
                if (BookingServices.Length > 0)
                {
                    //then re-add the services
                    booking.GroomServices = new List<GroomService>();
                    //then add services to that booking
                    foreach (int ServiceID in BookingServices)
                    {
                        //Debug.WriteLine("ServiceID is "+ServiceID);
                        GroomService service = db.GroomServices.Find(ServiceID);
                        //Debug.WriteLine("Service Name is "+service.ServiceName);
                        booking.GroomServices.Add(service);
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }


        public ActionResult Show(int id)
        {
            GroomBooking booking = db.GroomBookings.Include(b=>b.GroomServices).FirstOrDefault(b=>b.GroomBookingID==id);
            Debug.WriteLine("Logged in user is billed owner " + UserManager.IsUserBilledOwner(booking));
            return View(booking);
        }

        /////////////
        //how to get the UserManager and SignInManager from the server
        /////////////
        public GroomBookingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}